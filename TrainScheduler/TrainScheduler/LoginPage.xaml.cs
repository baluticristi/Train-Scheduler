using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TrainScheduler
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void usernameTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            usernameTextBox.Text = "";
        }

        private void passwordTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            passwordTextBox.Password = "";
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();

            window.Show();

            this.Close();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new RegisterPage();

            window.Show();

            this.Close();
        }
    }
}
