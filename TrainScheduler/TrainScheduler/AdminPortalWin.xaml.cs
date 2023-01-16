using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
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
        private User user;
        private TrainEntities context = new TrainEntities();
        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        public AdminPortalWin(User admin)
        {
            this.user = admin;
            InitializeComponent();
        }

       public void ShowAdminPort(bool isdark)
       {

            this.acceptBtn.Visibility = Visibility.Hidden;
            this.declineBtn.Visibility = Visibility.Hidden;
            this.requestTxTBoX.Visibility = Visibility.Hidden;
            this.reqLabel.Visibility = Visibility.Hidden;

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

            this.Show();
            refreshRequests();
       }
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();

            window.ShowMainWin(this.user, this.isDarkTheme);

            window.Show();

            this.Close();

        }

        private void refreshRequests()
        {


            var data = from r in context.Requests
                       join u in context.Users
                       on r.User_id equals u.User_id
                       join rol in context.Roles
                       on u.Role_id equals rol.Role_id 
                       select new
                       {
                           r.Request_id,
                           u.LastName,
                           u.FirstName,
                           rol.Role_name,
                           r.date
                       };

            var listRequest = data.ToList();

            if(data != null)
            {
                this.acceptBtn.Visibility = Visibility.Visible;
                this.declineBtn.Visibility = Visibility.Visible;
                this.requestTxTBoX.Visibility = Visibility.Visible;
                this.reqLabel.Visibility = Visibility.Visible;

                requestGrid.ItemsSource = listRequest;
            }

        }

        int getUserId()
        {
            int id = Convert.ToInt32(requestTxTBoX.Text);  //id reqeust

            var data = from r in context.Requests
                       join u in context.Users
                       on r.User_id equals u.User_id
                       where r.Request_id == id
                       select new
                       {
                           u.User_id
                       };

            if (data == null)
                return -1;

            return data.ToList().First().User_id;

        }

        private void deleteRequest(int req_id)
        {
            Request req = context.Requests.Where(r => r.Request_id == req_id).First();
            context.Requests.Remove(req);
            context.SaveChanges();

        }
        private void acceptBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(requestTxTBoX.Text);  //id reqeust
            int u_id = getUserId();

            if (u_id == -1)
                return;

            User user = context.Users.Where(u => u.User_id == u_id).First();
            user.is_verified = true;
            context.SaveChanges();
            deleteRequest(id);

            //MessageBox.Show("User was accepted!");
            MessageBoxWin mbox = new MessageBoxWin();
            mbox.msg.Text = "User was accepted!";
            mbox.ShowDialog();

            refreshRequests();

        }
        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();

            window.ShowMainWin(this.user, this.isDarkTheme);

            this.Close();

        }
        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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




        private void declineBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(requestTxTBoX.Text);  //id reqeust
            int u_id = getUserId();

            if (u_id == -1)
                return;

            User user = context.Users.Where(u => u.User_id == u_id).First();
            
            user.is_verified = true;
            user.Role_id = context.Roles.Where(r => r.Role_name == "Thief").First().Role_id;
            context.SaveChanges();
            deleteRequest(id);

            //MessageBox.Show("User was not accepted!");

            MessageBoxWin mbox = new MessageBoxWin();
            mbox.msg.Text = "User was not accepted!";
            mbox.ShowDialog();

            refreshRequests();

        }

        private void ticketBtnWin_Click(object sender, RoutedEventArgs e)
        {
            AllTicketWin tkWin = new AllTicketWin();
            
            tkWin.ShowTicketsWin(this.user, this.isDarkTheme);

            this.Close();
        }
    }
}
