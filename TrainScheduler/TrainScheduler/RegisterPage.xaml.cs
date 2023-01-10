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
using System.Security.Cryptography;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Data.SqlClient;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls.Primitives;

namespace TrainScheduler
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        private TrainEntities context = new TrainEntities();

        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        public RegisterPage()
        {
            InitializeComponent();
        }

   
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
        
        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            firstNameRegBox.Text = "";
        }

        private void lastNameRegBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            lastNameRegBox.Text = "";
        }

        private void emailRegBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            emailRegBox.Text = "";
        }

        private void phoneNumberRegBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            phoneNumberRegBox.Text = "";
        }

        private void passwordRegBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //
        }

       
        private void phoneNumberRegBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit((char)e.Text[0]))
            {
                e.Handled = true;
            }
        }
        private bool verifyEmailUnicity(string email)
        {
            var emails = from em in context.Users
                         select new
                         { 
                             em.email
                         };

            var list = emails.ToList();

            foreach(var item in list)
            {
                if (email == item.email)
                {
                    MessageBox.Show("A user with this email is already registered!");
                    return false;
                }
            }
            return true;
        }
        private bool verifyInputData()
        {
            if(firstNameRegBox.Text == "" || lastNameRegBox.Text == "" || emailRegBox.Text == "" || phoneNumberRegBox.Text ==  ""
                || passwordRegBox.ToString() == ""|| firstNameRegBox.Text == "First Name" || lastNameRegBox.Text == "Last Name" || emailRegBox.Text == "Email" || phoneNumberRegBox.Text == "phone"
                || passwordRegBox.ToString() == "password")
            {
                MessageBox.Show("All the fields must be filled!");
                return false;
            }

            return true;
        }

        public static byte[] getHashForPasswd(string pass)
        {
            byte[] salt = new byte[] { 0x20, 0x15, 0x09, 0x1B, 0x3D, 0x2E, 0x4F, 0x65,
                           0x7A, 0x89, 0x0C, 0xAB, 0xDE, 0xF1, 0x12, 0x34 };

            Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(pass, salt, 10000);
            byte[] key = keyGenerator.GetBytes(32); // 32-byte key

            return key;
        }

        private int getRoleId(string role)
        {
            var data = context.Roles.Where(item => item.Role_name == role).First();
           
            return data.Role_id;
        }
        private User getUserObject()
        {
            User user = new User();

            user.FirstName = firstNameRegBox.Text;
            user.LastName = lastNameRegBox.Text;
            user.phone = phoneNumberRegBox.Text;
            user.email = emailRegBox.Text;
            user.password = Convert.ToBase64String(getHashForPasswd(passwordRegBox.Password));
            user.is_verified = false;

            if (studentRegBox.IsChecked == true && elderRegBox.IsChecked == true)
                return null; 

            if (studentRegBox.IsChecked == true)
                user.Role_id = getRoleId("Student");
            else if (elderRegBox.IsChecked == true)
                user.Role_id = getRoleId("Elder");
            else
                user.Role_id = getRoleId("Adult");
          

            return user;
        }

        private Request getRequest(User user)
        {
            Request request = new Request();

            request.User_id = user.User_id;
            request.date = DateTime.Now;

            return request;
        }

        private void registerButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (verifyInputData() == true && verifyEmailUnicity(emailRegBox.Text) == true)
            {
                User user = getUserObject();

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Users.Add(user);
                        context.Requests.Add(getRequest(user));
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        if (studentRegBox.IsChecked == true && elderRegBox.IsChecked == true)
                            MessageBox.Show("You can not be a student and a senior citizen at the same time :(");
                        else
                            MessageBox.Show("The registration failed!");
                        return;
                    }
                }

                MessageBox.Show("You have been registrated!");

                var log_win = new LoginPage();
                log_win.Show();
                this.Close();

            }
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            var window = new LoginPage();

            window.showLoginPage(isDarkTheme);

            this.Close();
        }

        public void showRegWin(bool isdark)
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
                themeToggle.IsChecked = true;
                theme.SetBaseTheme(Theme.Dark);
            }
            paletteHelper.SetTheme(theme);
        }
    }
}
