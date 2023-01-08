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

        public void ShowBookingHistoryWin(User user)
        {
            this.user = user;
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
            mainWin.ShowMainWin(this.user);
            this.Close();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWin = new MainWindow();
            mainWin.ShowMainWin(this.user);
            this.Close();
        }

        private void ticketsBtn_Click(object sender, RoutedEventArgs e)
        {
            var data = from t in context.Tickets
                       join w in context.Wagons
                       on t.Wagon_id equals w.Wagon_id

                       join tr in context.Trains
                       on w.Train_id equals tr.Train_id

                       join ln in context.Lines
                       on tr.Line_id equals ln.Line_id

                       join ls1 in context.LineStations
                       on ln.Line_id equals ls1.Line_id

                       join st1 in context.Stations
                       on ls1.Station_id equals st1.Station_id

                       join ls2 in context.LineStations
                       on ln.Line_id equals ls2.Line_id

                       join st2 in context.Stations
                       on ls2.Station_id equals st2.Station_id

                       where ls1.Station_id == t.DepartureStation_id && ls2.Station_id == t.ArrivalStation_id

                       select new
                       {
                           Departure_Station = st1.Name,
                           Arrival_Station = st2.Name,
                           Departure_time = ls1.DepartureTime,
                           Arrival_time = ls2.ArrivalTime,
                           Train_Number = tr.Train_id,
                           Wagon_number = w.Wagon_id,
                           Date = t.DayAndTime
                       };

            ticketsGrid.ItemsSource = data.ToList();
        }
    }
}
