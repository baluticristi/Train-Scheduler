using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrainScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private User user;
        private TrainEntities context = new TrainEntities();
        public void ShowMainWin(string email, User user)
        {
            String str = "Bine ai venit, ";
            welcomeLabel.Content = str + email;
            this.user = user;

            hideAdminButton();

            this.Show();
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool isRole(string str_role)
        {
            var role = context.Roles.Where(r => r.Role_id == user.Role_id).First();
            if (role.Role_name == str_role)
                return true;
            return false;
        }
        public void hideAdminButton()
        {
            if (isRole("Administrator") == false)
                adminPortalBtn.Visibility = Visibility.Hidden;
        }
        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            var window = new LoginPage();

            window.Show();

            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void adminPortalBtn_Click(object sender, RoutedEventArgs e)
        {
            var adminWin = new AdminPortalWin(user);

            adminWin.ShowAdminPort();

            this.Close();
        }
    }
}
