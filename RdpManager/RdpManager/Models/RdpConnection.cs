namespace RdpManager.Models
{
    public class RdpConnection
    {
        public RdpConnection(string name, string address, string username, string password, string notes)
        {
            Name = name;
            Address = address;
            Username = username;
            Password = password;
            Notes = notes;
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Notes { get; set; }
    }
}
