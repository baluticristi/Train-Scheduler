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
        public void ShowMainWin(string email)
        {
            String str = "Bine ai venit, ";
            welcomeLabel.Content = str + email;

            this.Show();
        }
        public MainWindow()
        {
            InitializeComponent();
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
    }
}
