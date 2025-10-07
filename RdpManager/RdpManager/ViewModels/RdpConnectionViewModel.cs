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

            _name = _connection.Name;
            _address = _connection.Address;
            _username = _connection.Username;
            _password = _connection.Password;

            _isEditMode = false;
            _isPasswordVisible = false;

            ConnectCommand = new RelayCommand(_ => Connect());
            DeleteRdpConnectionCommand = new RelayCommand(_ => DeleteRdpConnection());
            EditRdpConnectionCommand = new RelayCommand(_ => EditRdpConnection());
            SaveEditRdpConnectionCommand = new RelayCommand(_ => SaveEditRdpConnection());
            CancelEditRdpConnectionCommand = new RelayCommand(_ => CancelEditRdpConnection());
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

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                if (_isPasswordVisible != value)
                {
                    _isPasswordVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
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

        public ICommand EditRdpConnectionCommand { get; }

        private void EditRdpConnection()
        {
            IsEditMode = true;
        }

        public ICommand SaveEditRdpConnectionCommand { get; }

        private void SaveEditRdpConnection()
        {
            if (Name == string.Empty
                || Address == string.Empty)
            {
                return;
            }

            var oldName = _connection.Name;
            var oldAddress = _connection.Address;
            var oldUsername = _connection.Username;
            var oldPassword = _connection.Password;
            _connection.Name = Name;
            _connection.Address = Address;
            _connection.Username = Username;
            _connection.Password = Password;
            FileHelper.AddOrUpdateRdpFile(Connection);
            if (oldName != Name)
            {
                FileHelper.DeleteRdpFile(oldName);
            }
            if (oldAddress != Address || oldUsername != Username || oldPassword != Password)
            {
                CredentialsHelper.DeleteCredentials(oldAddress);
                CredentialsHelper.StoreCredentials(Address, Username, Password);
            }

            IsEditMode = false;
        }

        public ICommand CancelEditRdpConnectionCommand { get; }

        private void CancelEditRdpConnection()
        {
            Name = _connection.Name;
            Address = _connection.Address;
            Username = _connection.Username;
            Password = _connection.Password;
            IsEditMode = false;
        }
    }
}
