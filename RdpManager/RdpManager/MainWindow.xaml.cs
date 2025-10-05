using RdpManager.ViewModels;
using System.Windows;

namespace RdpManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(ResetSelectedRdpConnection);
        }

        private void ResetSelectedRdpConnection() => ConnectionsListBox.SelectedItem = null;
    }
}