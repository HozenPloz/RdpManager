namespace RdpManager.Models
{
    public class RdpConnection
    {
        public RdpConnection(string name, string address, string username, string password)
        {
            Name = name;
            Address = address;
            Username = username;
            Password = password;
        }

        public RdpConnection()
        {
            Name = string.Empty;
            Address = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
