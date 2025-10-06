using System.Runtime.InteropServices;
using System.Text;

namespace RdpManager.Helpers
{
    public class CredentialsHelper
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CREDENTIAL
        {
            public uint Flags;
            public uint Type;
            public string TargetName;
            public string Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public uint CredentialBlobSize;
            public IntPtr CredentialBlob;
            public uint Persist;
            public uint AttributeCount;
            public IntPtr Attributes;
            public string TargetAlias;
            public string UserName;
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CredWrite([In] ref CREDENTIAL userCredential, [In] uint flags);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CredDelete(string target, uint type, uint flags);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CredRead(string target, uint type, uint flags, out IntPtr credentialPtr);

        [DllImport("advapi32.dll", SetLastError = false)]
        internal static extern void CredFree(IntPtr buffer);

        const uint CRED_TYPE_GENERIC = 1;
        const uint CRED_PERSIST_LOCAL_MACHINE = 2;

        private const string RDP_PREFIX = "TERMSRV/";

        public static void StoreCredentials(string address, string username, string password)
        {
            var target = RDP_PREFIX + address;
            byte[] byteArray = Encoding.Unicode.GetBytes(password);

            var credential = new CREDENTIAL
            {
                Flags = 0,
                Type = CRED_TYPE_GENERIC,
                TargetName = target,
                Comment = null,
                LastWritten = new System.Runtime.InteropServices.ComTypes.FILETIME(),
                CredentialBlobSize = (uint)byteArray.Length,
                CredentialBlob = Marshal.AllocCoTaskMem(byteArray.Length),
                Persist = CRED_PERSIST_LOCAL_MACHINE,
                AttributeCount = 0,
                Attributes = IntPtr.Zero,
                TargetAlias = null,
                UserName = username
            };

            Marshal.Copy(byteArray, 0, credential.CredentialBlob, byteArray.Length);

            bool written = CredWrite(ref credential, 0);
            Marshal.FreeCoTaskMem(credential.CredentialBlob);

            if (!written)
            {
                int err = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(err, "CredWrite failed");
            }
        }

        public static void DeleteCredentials(string address)
        {
            var target = RDP_PREFIX + address;
            _ = CredDelete(target, CRED_TYPE_GENERIC, 0);
        }

        public static bool TryReadCredentials(string address, out string username, out string password)
        {
            username = string.Empty;
            password = string.Empty;
            var target = RDP_PREFIX + address;
            IntPtr credPtr;
            if (!CredRead(target, CRED_TYPE_GENERIC, 0, out credPtr))
            {
                _ = Marshal.GetLastWin32Error();
                return false;
            }

            try
            {
                var credential = (CREDENTIAL)Marshal.PtrToStructure(credPtr, typeof(CREDENTIAL));

                username = credential.UserName;

                if (credential.CredentialBlobSize > 0 && credential.CredentialBlob != IntPtr.Zero)
                {
                    byte[] blob = new byte[credential.CredentialBlobSize];
                    Marshal.Copy(credential.CredentialBlob, blob, 0, blob.Length);
                    password = Encoding.Unicode.GetString(blob).TrimEnd('\0');
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                CredFree(credPtr);
            }

            return true;
        }
    }
}