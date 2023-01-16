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
    /// Interaction logic for AllTicketWin.xaml
    /// </summary>
    public partial class AllTicketWin : Window
    {
        private TrainEntities context = new TrainEntities();
        User admin;

        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        public AllTicketWin()
        {
            InitializeComponent();
        }

        public void ShowTicketsWin(User admin, bool isdark)
        {
          
            this.admin= admin;

            fillUserComboBox();

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
            AdminPortalWin adminWin = new AdminPortalWin(this.admin);
            adminWin.Show();
            this.Close();
        }

        private void fillUserComboBox()
        {
            var users = from u in context.Users
                        where u.FirstName != "" && u.LastName != ""
                        select new
                        {
                            User_Name = u.FirstName + " " + u.LastName
                        };

            foreach (var item in users.ToList())
            {
                comboUsers.Items.Add(item.User_Name);
            }

        }

        private User GetUser(string name)
        {
            var user = context.Users.Where(u => u.FirstName+" "+u.LastName == name).First();
            return user;
        }

        private void comboUsers_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            User user = GetUser(comboUsers.SelectedValue.ToString());

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

                              where user.User_id == t.User_id && t.DepartureStation_id == st1.Station_id && t.ArrivalStation_id == st2.Station_id

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

            ticketsGrid.ItemsSource = ticketsData.ToList();
        }
    }
}
