using System.Windows;
using System.Windows.Controls;

namespace CustomControls
{
    /// <summary>
    /// Interaction logic for CustomPasswordBox.xaml
    /// </summary>
    public partial class CustomPasswordBox : UserControl
    {
        public CustomPasswordBox()
        {
            InitializeComponent();
            PasswordBox.Password = Password;
        }

        #region Properties

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password), typeof(string), typeof(CustomPasswordBox),
                new PropertyMetadata(""));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty IsPasswordVisibleProperty =
            DependencyProperty.Register(
                nameof(IsPasswordVisible), typeof(bool), typeof(CustomPasswordBox),
                new PropertyMetadata(false));

        public bool IsPasswordVisible
        {
            get { return (bool)GetValue(IsPasswordVisibleProperty); }
            set { SetValue(IsPasswordVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register(
                nameof(IsEditable), typeof(bool), typeof(CustomPasswordBox),
                new PropertyMetadata(false));

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        #endregion

        #region Event handlers

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Password != ((PasswordBox)sender).Password)
            {
                Password = ((PasswordBox)sender).Password;
            }
        }

        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            IsPasswordVisible = true;
        }

        private void HidePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            IsPasswordVisible = false;
        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordBox.Password != ((TextBox)sender).Text)
            {
                PasswordBox.Password = ((TextBox)sender).Text;
            }
        }

        #endregion
    }
}
