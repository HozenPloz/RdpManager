using RdpManager.Commands;
using RdpManager.Models;
using System.Windows.Input;

namespace RdpManager.ViewModels
{
    public class AddRdpConnectionViewModel : ViewModelBase
    {
        private AddRdpConnectionViewModel()
        {
            AddRdpConnectionCommand = new RelayCommand(_ => AddRdpConnection());
        }

        private static AddRdpConnectionViewModel? _instance;
        public static AddRdpConnectionViewModel GetInstance() => _instance ??= new AddRdpConnectionViewModel();

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
            // TODO: Implement add connection
            if (Name == string.Empty
                || Address == string.Empty
                || Username == string.Empty
                || Password == string.Empty)
            {
                return;
            }

            var newConnection = new RdpConnectionViewModel(new RdpConnection(Name, Address, Username, Password));
            MainWindowViewModel.Connections.Add(newConnection);

            Name = string.Empty;
            Address = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}
