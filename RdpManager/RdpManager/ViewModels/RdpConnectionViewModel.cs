using RdpManager.Commands;
using RdpManager.Helpers;
using RdpManager.Models;
using System.Diagnostics;
using System.Windows;
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
            DeleteRdpConnectionCommand = new RelayCommand(_ => DeleteRdpConnection());
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
            var path = FileHelper.GetRdpFilePath(Name);
            Process.Start("mstsc.exe", path);
        }

        public ICommand DeleteRdpConnectionCommand { get; }

        private void DeleteRdpConnection()
        {
            var result = MessageBox.Show(
                "Are you sure that you want to delete the RDP connection?",
                "confirmation",
                MessageBoxButton.YesNo
            );

            if (result == MessageBoxResult.Yes)
            {
                MainWindowViewModel.Connections.Remove(this);
                CredentialsHelper.DeleteCredentials(Address);
                FileHelper.DeleteRdpFile(Name);
            }
        }
    }
}
