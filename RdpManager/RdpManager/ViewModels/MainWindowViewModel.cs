using RdpManager.Commands;
using RdpManager.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RdpManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static ViewModelBase? _selectedViewModel;
        public static ObservableCollection<RdpConnectionViewModel> Connections { get; set; } = new();

        private Action ResetSelectedRdpConnection;


        public ViewModelBase? SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                if (_selectedViewModel != value)
                {
                    _selectedViewModel = value;
                    OnPropertyChanged();
                }
            }
        }


        public MainWindowViewModel(Action resetSelectedRdpConnection)
        {
            // TODO: Save connections in a encrypted file
            Connections.Add(new RdpConnectionViewModel(new RdpConnection("Server 1", "192.168.0.10", "admin", "pass")));
            Connections.Add(new RdpConnectionViewModel(new RdpConnection("Server 2", "192.168.0.20", "user", "1234")));

            SelectedViewModel = Connections.FirstOrDefault();

            ShowAddRdpConnectionCommand = new RelayCommand(_ => ShowAddRdpConnection());

            ResetSelectedRdpConnection = resetSelectedRdpConnection;
        }

        public ICommand ShowAddRdpConnectionCommand { get; }

        private void ShowAddRdpConnection()
        {
            ResetSelectedRdpConnection();
            SelectedViewModel = AddRdpConnectionViewModel.GetInstance();
        }
    }
}
