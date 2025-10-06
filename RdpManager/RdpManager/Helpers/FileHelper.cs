using RdpManager.Models;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RdpManager.Helpers
{
    public class FileHelper
    {
        private const string FilePath = "RdpConnections.txt";

        public static void SaveRdpConnections(List<RdpConnection> connections)
        {
            try
            {
                var stringifiedConnections = connections.Select(c => $"{c.Name}:{c.Address}:{c.Username}:{c.Password}");
                var combined = string.Join(";", stringifiedConnections);
                var data = Encoding.UTF8.GetBytes(combined);
                var encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(FilePath, encrypted);
            }
            catch
            {
            }
        }

        public static List<RdpConnection> LoadRdpConnections()
        {
            if (!File.Exists(FilePath))
            {
                return new List<RdpConnection>();
            }

            string combined;
            try
            {
                byte[] encrypted = File.ReadAllBytes(FilePath);
                byte[] decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                combined = Encoding.UTF8.GetString(decrypted);
            }
            catch
            {
                return new List<RdpConnection>();
            }

            var connections = new List<RdpConnection>();
            var stringifiedConnections = combined.Split(';');
            foreach (var connection in stringifiedConnections)
            {
                var parts = connection.Split(':');
                if (parts.Length == 4)
                {
                    string name = parts[0];
                    string address = parts[1];
                    string username = parts[2];
                    string password = parts[3];
                    connections.Add(new RdpConnection(name, address, username, password));
                }
            }

            return connections;
        }
    }
}