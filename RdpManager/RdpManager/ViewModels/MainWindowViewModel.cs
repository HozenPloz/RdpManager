using RdpManager.Models;
using System.Collections.ObjectModel;

namespace RdpManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private RdpConnectionViewModel? _selectedConnection;
        public ObservableCollection<RdpConnectionViewModel> Connections { get; } = new();


        public RdpConnectionViewModel? SelectedConnection
        {
            get => _selectedConnection;
            set
            {
                if (_selectedConnection != value)
                {
                    _selectedConnection = value;
                    OnPropertyChanged();
                }
            }
        }


        public MainWindowViewModel()
        {
            // TODO: Save connections in a encrypted file
            Connections.Add(new RdpConnectionViewModel(new RdpConnection { Name = "Server 1", Address = "192.168.0.10", Username = "admin", Password = "pass" }));
            Connections.Add(new RdpConnectionViewModel(new RdpConnection { Name = "Server 2", Address = "192.168.0.20", Username = "user", Password = "1234" }));

            SelectedConnection = Connections.FirstOrDefault();
        }
    }
}
