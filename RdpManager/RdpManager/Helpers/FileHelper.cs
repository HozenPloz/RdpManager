using RdpManager.Models;
using System.IO;

namespace RdpManager.Helpers
{
    public class FileHelper
    {
        private const string RdpFolder = "./RdpFiles/";

        public static void SaveRdpConnections(List<RdpConnection> connections)
        {
            foreach (var connection in connections)
            {
                CreateRdpFile(connection.Address, connection.Username, connection.Name);
            }
        }

        private static void CreateRdpFile(string address, string username, string name)
        {
            string rdpContent =
$@"full address:s:{address}
username:s:{username}
prompt for credentials:i:1
screen mode id:i:2
desktopwidth:i:1920
desktopheight:i:1080
redirectdrives:i:1
redirectprinters:i:1
enablecredsspsupport:i:1";

            try
            {
                if (!Directory.Exists(RdpFolder))
                {
                    Directory.CreateDirectory(RdpFolder);
                }

                var path = Path.Combine(RdpFolder, $"{name}.rdp");

                File.WriteAllText(path, rdpContent);
            }
            catch
            {
            }
        }

        public static string GetRdpFilePath(string name) => Path.Combine(RdpFolder, $"{name}.rdp");

        public static List<RdpConnection> LoadRdpConnections()
        {
            if (!Directory.Exists(RdpFolder))
            {
                return new List<RdpConnection>();
            }

            var connections = new List<RdpConnection>();
            try
            {
                string[] rdpFiles = Directory.GetFiles(RdpFolder, "*.rdp", SearchOption.TopDirectoryOnly);
                foreach (var file in rdpFiles)
                {
                    var name = Path.GetFileNameWithoutExtension(file);
                    if (!TryReadAddressAndUsernameFromRdpFile(file, out string address, out string username))
                    {
                        continue;
                    }

                    if (!CredentialsHelper.TryReadCredentials(address, out var credentialsUsername, out var password))
                    {
                        continue;
                    }

                    if (credentialsUsername != username)
                    {
                        CredentialsHelper.StoreCredentials(address, username, password);
                    }

                    connections.Add(new RdpConnection(name, address, username, password));
                }
            }
            catch
            {
                return new List<RdpConnection>();
            }


            return connections;
        }

        private static bool TryReadAddressAndUsernameFromRdpFile(string filePath, out string address, out string username)
        {
            address = string.Empty;
            username = string.Empty;
            var usernameSet = false;
            try
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (trimmedLine.StartsWith("full address:s:"))
                    {
                        address = trimmedLine.Substring("full address:s:".Length).Trim();
                    }

                    if (trimmedLine.StartsWith("username:s:"))
                    {
                        username = trimmedLine.Substring("username:s:".Length).Trim();
                        usernameSet = true;
                    }

                    if (!string.IsNullOrWhiteSpace(address) && usernameSet)
                    {
                        break;
                    }
                }
            }
            catch
            {
            }

            return !string.IsNullOrWhiteSpace(address) && usernameSet;
        }
    }
}