using MaterialDesignThemes.Wpf;
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

        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
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


        private void registerButton_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new RegisterPage();

            window.showRegWin(isDarkTheme);

            this.Close();
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void showLoginPage(bool isdark)
        {
            this.Show();

            ITheme theme = paletteHelper.GetTheme();

            if (isdark == false)
            {
                isDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                isDarkTheme = true;
                themeToggle.IsChecked = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();

            if (isDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark)
            {
                isDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
            }
            else
            {
                isDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void log_in()
        {
            if (verifiyCredentials() == true)
            {
                var window = new MainWindow();
                this.user = getUser(usernameTextBox.Text);
                window.ShowMainWin(this.user, this.isDarkTheme);
                this.Close();

            }
            else
               MessageBox.Show("Invalid Credentials");
              
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            log_in();

            //this.Close();
        }

        private void passwordBox_keyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                log_in();
            }
        }
    }
}
