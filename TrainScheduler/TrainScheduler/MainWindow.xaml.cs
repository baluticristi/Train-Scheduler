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
            hideSingleTrainSearch();
            showAllTrainGrid();

            var trainData = from t in context.Trains
                            join ls in context.LineStations
                            on t.Line_id equals ls.Line_id    //for trains ids

                            join s in context.Stations
                            on ls.Station_id equals s.Station_id  //for departure stations

                            join ls2 in context.LineStations
                            on t.Line_id equals ls2.Line_id

                            join s2 in context.Stations
                            on ls2.Station_id equals s2.Station_id

                            where ls.ArrivalTime == null && ls2.DepartureTime == null
                            select new
                            {
                                t.Train_id,
                                DepartureStation = s.Name,
                                ArrivalStation = s2.Name
                                DepartureTime = ls.DepartureTime

                            };

            allTrainGrid.ItemsSource = trainData.ToList();
        }

        private void adminPortalBtn_Click(object sender, RoutedEventArgs e)
        {
            var adminWin = new AdminPortalWin(user);

            adminWin.ShowAdminPort();

            this.Close();
        }

        private void hideSingleTrainSearch()
        {
            trainNumberLabel.Visibility = Visibility.Hidden;
            trainNumberTxTBox.Visibility = Visibility.Hidden;
            searchSingleTrain.Visibility = Visibility.Hidden;
            singleTrainGrid.Visibility = Visibility.Hidden;
        }

        private void showSingleTrainSearch()
        {
            trainNumberLabel.Visibility = Visibility.Visible;
            trainNumberTxTBox.Visibility = Visibility.Visible;
            searchSingleTrain.Visibility = Visibility.Visible;
            singleTrainGrid.Visibility = Visibility.Visible;
        }

        private void hideAllTrainGrid()
        {
            allTrainGrid.Visibility = Visibility.Hidden;
        }

        private void showAllTrainGrid()
        {
            allTrainGrid.Visibility = Visibility.Visible;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            hideAllTrainGrid();
            showSingleTrainSearch();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int trainNumberInt = Convert.ToInt32(trainNumberTxTBox.Text);

            var trainData = from t in context.Trains
                            join ls in context.LineStations
                            on t.Line_id equals ls.Line_id    //for trains ids

                            join s in context.Stations
                            on ls.Station_id equals s.Station_id  //for departure stations

                            join ls2 in context.LineStations
                            on t.Line_id equals ls2.Line_id

                            join s2 in context.Stations
                            on ls2.Station_id equals s2.Station_id

                            where ls.ArrivalTime == null && ls2.DepartureTime == null && t.Train_id == trainNumberInt
                            select new
                            {
                                t.Train_id,
                                DepartureStation = s.Name,
                                ArrivalStation = s2.Name

                            };

            if (trainData != null)
            {
                singleTrainGrid.Visibility = Visibility.Visible;
                singleTrainGrid.ItemsSource = trainData.ToList();
            }
            else
                return;
        }
    }
}