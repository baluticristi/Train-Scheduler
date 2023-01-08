using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
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

        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        public void ShowMainWin(User user)
        {
            String str = "Bine ai venit, ";
            this.user = user;
            welcomeLabel.Content = str + user.FirstName;


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
        private void ShowAllTrains()
        {
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
                                ArrivalStation = s2.Name,
                                DepartureTime = ls.DepartureTime

                            };

            allTrainGrid.ItemsSource = trainData.ToList();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            hideSingleTrainSearch();
            showAllTrainGrid();

            this.ShowAllTrains();
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

        static bool isJustDigits(string input)
        {
            bool isDigit = Regex.IsMatch(input, @"^\d+$");
            return isDigit;
        }
        private void ShowSingleTrain()
        {
            if(isJustDigits(trainNumberTxTBox.Text) == false)
            {
                MessageBox.Show($"The train with the id: {trainNumberTxTBox.Text} does not exist!");
                return;
            }
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

            if (trainData.ToList().Count() != 0)
            {
                singleTrainGrid.Visibility = Visibility.Visible;
                singleTrainGrid.ItemsSource = trainData.ToList();
            }
            else
            {
                MessageBox.Show($"The train with the id: {trainNumberTxTBox.Text} does not exist!");
                return;
            };
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.ShowSingleTrain();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var tripWin = new TripPlanner();
            tripWin.showTripPlanWin(this.user);
            this.Close();
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

        private void exitApp(object sender, RoutedEventArgs e)
        {
            var window = new LoginPage();

            window.Show();

            this.Close();
        }
    }
}