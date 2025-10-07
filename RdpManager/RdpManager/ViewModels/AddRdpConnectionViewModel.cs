using RdpManager.Commands;
using RdpManager.Helpers;
using RdpManager.Models;
using System.IO;
using System.Windows.Input;

namespace RdpManager.ViewModels
{
    public class AddRdpConnectionViewModel : ViewModelBase
    {
        private AddRdpConnectionViewModel(Action<ViewModelBase> setSelectedViewModel)
        {
            SetSelectedViewModel = setSelectedViewModel;
            AddRdpConnectionCommand = new RelayCommand(_ => AddRdpConnection());
            ImportRdpFileCommand = new RelayCommand(_ => ImportRdpFile());
        }

        private Action<ViewModelBase> SetSelectedViewModel;

        private static AddRdpConnectionViewModel? _instance;
        public static AddRdpConnectionViewModel GetInstance(Action<ViewModelBase> setSelectedViewModel) => _instance ??= new AddRdpConnectionViewModel(setSelectedViewModel);

        private string ImportedRdpFileContent = string.Empty;

        private string _name = string.Empty;
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

        private string _address = string.Empty;
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

        private string _username = string.Empty;
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

        private string _password = string.Empty;
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

        public ICommand AddRdpConnectionCommand { get; }

        private void AddRdpConnection()
        {
            if (Name == string.Empty
                || Address == string.Empty
                || Username == string.Empty
                || Password == string.Empty)
            {
                return;
            }

            if (MainWindowViewModel.Connections.Any(c => c.Connection.Name == Name))
            {
                return;
            }

            var newConnection = new RdpConnection(Name, Address, Username, Password);
            var newConnectionViewModel = new RdpConnectionViewModel(newConnection);
            MainWindowViewModel.Connections.Add(newConnectionViewModel);
            FileHelper.AddOrUpdateRdpFile(newConnection, ImportedRdpFileContent);
            CredentialsHelper.DeleteCredentials(Address);
            CredentialsHelper.StoreCredentials(Address, Username, Password);

            SetSelectedViewModel(newConnectionViewModel);

            Name = string.Empty;
            Address = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }

        public ICommand ImportRdpFileCommand { get; }

        private void ImportRdpFile()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "RDP file (*.rdp)|*.rdp",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            var filename = openFileDialog.FileNames.FirstOrDefault();
            if (filename == null)
            {
                return;
            }

            _ = FileHelper.TryReadAddressAndUsernameFromRdpFile(filename, out var address, out var username);
            Address = address;
            Username = username;
            Name = Path.GetFileNameWithoutExtension(filename);
            ImportedRdpFileContent = File.ReadAllText(filename);
        }
    }
}
