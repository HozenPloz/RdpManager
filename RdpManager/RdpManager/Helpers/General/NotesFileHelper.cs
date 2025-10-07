using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace RdpManager.Helpers.General
{
    public class NotesFileHelper
    {
        private const string FilePath = "./RdpConnectionNotes.txt";

        public static void SaveConnectionNotes(Dictionary<string, string> connectionNotes)
        {
            try
            {
                var json = JsonSerializer.Serialize(connectionNotes);
                var data = Encoding.UTF8.GetBytes(json);
                var encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(FilePath, encrypted);
            }
            catch
            {
            }
        }

        public static void AddOrUpdateConnectionNote(string connectionName, string note)
        {
            var connectionNotes = LoadConnectionNotes();
            connectionNotes[connectionName] = note;
            SaveConnectionNotes(connectionNotes);
        }

        public static void DeleteConnectionNote(string connectionName)
        {
            var connectionNotes = LoadConnectionNotes();
            if (connectionNotes.Remove(connectionName))
            {
                SaveConnectionNotes(connectionNotes);
            }
        }

        public static Dictionary<string, string> LoadConnectionNotes()
        {
            if (!File.Exists(FilePath))
            {
                return new Dictionary<string, string>();
            }

            string json;
            try
            {
                byte[] encrypted = File.ReadAllBytes(FilePath);
                byte[] decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                json = Encoding.UTF8.GetString(decrypted);
            }
            catch
            {
                return new Dictionary<string, string>();
            }

            try
            {
                var connectionNotes = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                return connectionNotes ?? new Dictionary<string, string>();
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }
    }
}
