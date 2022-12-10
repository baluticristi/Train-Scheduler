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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

   
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            var window = new LoginPage();

            window.Show();

            this.Close();
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
            passwordRegBox.Text = "";
        }

        private void phoneNumberRegBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit((char)e.Text[0]))
            {
                e.Handled = true;
            }
        }
    }
}
