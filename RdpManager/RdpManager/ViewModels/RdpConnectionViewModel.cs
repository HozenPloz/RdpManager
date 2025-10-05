using RdpManager.Commands;
using RdpManager.Models;
using System.Windows.Input;

namespace RdpManager.ViewModels
{
    public class RdpConnectionViewModel : ViewModelBase
    {
        private RdpConnection _connection;

        public RdpConnectionViewModel(RdpConnection connection)
        {
            _connection = connection;

            ConnectCommand = new RelayCommand(_ => Connect());
        }

        public RdpConnection Connection
        {
            get => _connection;
            set
            {
                if (_connection != value)
                {
                    _connection = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _connection.Name;
            set
            {
                if (_connection.Name != value)
                {
                    _connection.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => _connection.Address;
            set
            {
                if (_connection.Address != value)
                {
                    _connection.Address = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Username
        {
            get => _connection.Username;
            set
            {
                if (_connection.Username != value)
                {
                    _connection.Username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _connection.Password;
            set
            {
                if (_connection.Password != value)
                {
                    _connection.Password = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ConnectCommand { get; }

        private void Connect()
        {
            // TODO: Implement RDP connection logic
        }
    }
}
