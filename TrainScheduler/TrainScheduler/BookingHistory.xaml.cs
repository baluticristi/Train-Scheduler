using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
    /// Interaction logic for BookingHistory.xaml
    /// </summary>
    public partial class BookingHistory : Window
    {
        private User user;
        private TrainEntities context = new TrainEntities();

        public BookingHistory()
        {
            InitializeComponent();
        }

        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        public void ShowBookingHistoryWin(User user, bool isdark)
        {
            this.user = user;

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
            var mainWin = new MainWindow();
            mainWin.ShowMainWin(this.user, this.isDarkTheme);
            this.Close();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWin = new MainWindow();
            mainWin.ShowMainWin(this.user, this.isDarkTheme);
            this.Close();
        }

        private void ticketsBtn_Click(object sender, RoutedEventArgs e)
        {
            var ticketsData = from t in context.Tickets
                              join wag in context.Wagons
                              on t.Wagon_id equals wag.Wagon_id

                              join tr in context.Trains
                              on wag.Train_id equals tr.Train_id

                              join ls1 in context.LineStations
                              on tr.Line_id equals ls1.Line_id

                              join st1 in context.Stations
                              on ls1.Station_id equals st1.Station_id

                              join ls2 in context.LineStations
                              on tr.Line_id equals ls2.Line_id

                              join st2 in context.Stations
                              on ls2.Station_id equals st2.Station_id

                              where user.User_id == t.User_id && t.DepartureStation_id == st1.Station_id && t.ArrivalStation_id== st2.Station_id

                              select new
                              {
                                  Train_Number = tr.Train_id,
                                  Wagon_Number = wag.Wagon_id,
                                  Seat_Number = t.NumberOfSeat,
                                  Departure_Station = st1.Name,
                                  Arrival_Station = st2.Name,
                                  Departure_Time = ls1.DepartureTime,
                                  Arrival_Time = ls2.ArrivalTime,

                                  Trip_Date = t.DayAndTime
                              };

                                                                            //merge foarte bine, ai grija sa nu fie pb cu vagoanele harcodate.
            ticketsGrid.ItemsSource = ticketsData.ToList();
        }
    }
}
