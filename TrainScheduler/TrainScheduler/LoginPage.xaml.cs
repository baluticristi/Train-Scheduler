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
        private TrainEntities context = new TrainEntities();
        private User user;
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

        private bool verifiyCredentials()
        {
            var data = from u in context.Users
                       select new
                       {
                           u.email,
                           u.password
                       };

            var users = data.ToList();

            foreach (var user in users)
            {
                if(user.email.ToString() == usernameTextBox.Text && user.password.ToString() == Convert.ToBase64String(RegisterPage.getHashForPasswd(passwordTextBox.Password)))
                    return true;
            }
            return false;
        }

        private User getUser(string email)
        {
            var data = context.Users.Where(u => u.email == email).First();
            return data;
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {

            if (verifiyCredentials() == true)
            {
                var window = new MainWindow();
                this.user = getUser(usernameTextBox.Text);
                window.ShowMainWin(usernameTextBox.Text, this.user);
                this.Close();
           
            }
            else
                MessageBox.Show("Invalid Credentials");

            //this.Close();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new RegisterPage();

            window.Show();

            this.Close();
        }
    }
}
