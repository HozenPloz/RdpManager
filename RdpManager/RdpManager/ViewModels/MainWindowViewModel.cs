using RdpManager.Commands;
using RdpManager.Helpers.General;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RdpManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static ObservableCollection<RdpConnectionViewModel> Connections { get; set; } = new();

        private Action ResetSelectedRdpConnection;


        private ViewModelBase? _selectedViewModel;
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

        private void SetSelectedViewModel(ViewModelBase viewModel)
        {
            SelectedViewModel = viewModel;
        }

        public MainWindowViewModel(Action resetSelectedRdpConnection)
        {
            Connections = new ObservableCollection<RdpConnectionViewModel>(RdpFileHelper.LoadRdpConnections().Select(c => new RdpConnectionViewModel(c)));

            SelectedViewModel = Connections.FirstOrDefault();

            ShowAddRdpConnectionCommand = new RelayCommand(_ => ShowAddRdpConnection());

            ResetSelectedRdpConnection = resetSelectedRdpConnection;
        }

        public ICommand ShowAddRdpConnectionCommand { get; }

        private void ShowAddRdpConnection()
        {
            ResetSelectedRdpConnection();
            SelectedViewModel = AddRdpConnectionViewModel.GetInstance(SetSelectedViewModel);
        }
    }
}
