using RdpManager.ViewModels;
using System.Windows.Controls;

namespace RdpManager.Views
{
    /// <summary>
    /// Interaction logic for RdpConnectionView.xaml
    /// </summary>
    public partial class RdpConnectionView : UserControl
    {
        public RdpConnectionView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is RdpConnectionViewModel rdpConnectionViewModel
                && rdpConnectionViewModel.Password != ((PasswordBox)sender).Password)
            {
                rdpConnectionViewModel.Password = ((PasswordBox)sender).Password;
            }
        }

        private void PasswordBox_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is RdpConnectionViewModel rdpConnectionViewModel
                && ((PasswordBox)sender).Password != rdpConnectionViewModel.Password)
            {
                ((PasswordBox)sender).Password = rdpConnectionViewModel.Password;
            }
        }
    }
}
