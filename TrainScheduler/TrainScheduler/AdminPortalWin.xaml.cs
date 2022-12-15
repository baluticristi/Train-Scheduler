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
    /// Interaction logic for AdminPortalWin.xaml
    /// </summary>
    public partial class AdminPortalWin : Window
    {
        private User admin;
        private TrainEntities context = new TrainEntities();
        public AdminPortalWin(User admin)
        {
            this.admin = admin;
            InitializeComponent();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();

            window.ShowMainWin(this.admin.email, this.admin);

            window.Show();

            this.Close();

        }

        private void requestBtn_Click(object sender, RoutedEventArgs e)
        {
            var data = from r in context.Requests
                       select new
                       {
                           r.Request_id,
                           r.User_id,
                           r.date
                       };

            var listRequest = data.ToList();

            requestGrid.ItemsSource = listRequest;
        }
    }
}
