using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace TrainScheduler
{
    /// <summary>
    /// Interaction logic for TripPlanner.xaml
    /// </summary>
    public partial class TripPlanner : Window
    {
        private User user;
        private TrainEntities context = new TrainEntities();
        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        public TripPlanner()
        {
            InitializeComponent();
        }

        private void fillDepartureComboBox()
        {
            var trainData = from t in context.Trains
                            join ls in context.LineStations
                            on t.Line_id equals ls.Line_id    //for trains ids

                            join s in context.Stations
                            on ls.Station_id equals s.Station_id  //for departure stations

                            where ls.DepartureTime != null
                            select new 
                            {
                                DepartureStation = s.Name,
                            };
        
           foreach(var item in trainData.ToList())
           {
                departureCombo.Items.Add(item.DepartureStation);
           }
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

        private void fillArrivalComboBox()
        {
            string departure_st = departureCombo.Text;
            arrivalCombo.Items.Clear();

            var trainData = (from t in context.Trains
                             join ls in context.LineStations
                             on t.Line_id equals ls.Line_id    //for trains ids

                             join s in context.Stations
                             on ls.Station_id equals s.Station_id  //for departure stations

                             join ls2 in context.LineStations
                             on ls.Line_id equals ls2.Line_id

                             join s2 in context.Stations
                             on ls2.Station_id equals s2.Station_id

                             where ls2.ArrivalTime != null && ls.DepartureTime != null &&
                             ls.Distance < ls2.Distance && ls.Line_id == ls2.Line_id && s.Name == departure_st

                             select new
                             {
                                 ArrivalStation = s2.Name
                             });                       //NU MERGE DISTINCT?

            foreach (var item in trainData.ToList())
            {
                arrivalCombo.Items.Add(item.ArrivalStation);
            }
            arrivalCombo.SelectedIndex = 0;
        }
        public void showTripPlanWin(User user, bool isdark)
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
            fillDepartureComboBox();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }


        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();

            window.ShowMainWin(this.user, this.isDarkTheme);

            this.Close();

        }
        KeyValuePair<int, int> GetWagon(int id, string departure, string arrival, DateTime? time)

        {
            int WagNum;
            //get the wagons for the train on the route
            var wagons = from t in context.Trains
                            join ls in context.LineStations
                            on t.Line_id equals ls.Line_id    //for trains ids

                            join s in context.Stations
                            on ls.Station_id equals s.Station_id  //for departure stations

                            join ls2 in context.LineStations
                            on t.Line_id equals ls2.Line_id

                            join s2 in context.Stations
                            on ls2.Station_id equals s2.Station_id

                            join w in context.Wagons
                            on t.Train_id equals w.Train_id

                            where ls2.ArrivalTime != null && ls.DepartureTime != null &&
                            ls.Distance < ls2.Distance && ls.Line_id == ls2.Line_id && s.Name == departure && s2.Name == arrival &&
                            t.Train_id == id
                            select new
                            {
                                WagonNumber = w.Wagon_id,
                                Capacity=w.Capacity
                            };
            //cross check them with the tickets already bought
            var WagonsBooked = from tck in context.Tickets
                               join t in context.Wagons
                               on tck.Wagon_id equals t.Wagon_id

                               where t.Train_id == id
                               select new
                               {
                                   WagonID = tck.Wagon_id,
                                   ResDate = tck.DayAndTime
                               };

            Dictionary<int, int> wags = new Dictionary<int,int>();
            foreach (var w in wagons)
            {
                wags.Add(w.WagonNumber, Convert.ToInt32(w.Capacity));
            }

            foreach (var b in WagonsBooked)
            {
                for (int i = 0; i < wags.Count; i++)
                {
                    if (wags.ElementAt(i).Key == b.WagonID && b.ResDate==time)
                    {
                        wags[wags.ElementAt(i).Key] = wags[wags.ElementAt(i).Key]-1;
                    }
                }
            }

            foreach (int key in wags.Keys)
            {
                if (wags[key] > 0)
                {
                    
                    return  new KeyValuePair<int, int>(Convert.ToInt32(key),wags[key]);
                }
            }
            return new KeyValuePair<int, int> (0,0);
        }
        private void BookTicket(int id, string departure, string arrival,DateTime? time, double? Price) 
        {
            bool skip = false;
            if (time == null || time < DateTime.Now)
            {
                MessageBox.Show("Incorrect Date!");
                return;
            }
            if (user.Role.Role_name == "Student") { Price = Price / 2; }
            else if (user.Role.Role_name == "Elder") { Price = Price / 3; }
            else if (user.Role.Role_name == "Administator") { Price = 0; }
            else if (user.Role.Role_name == "Thief")
            {
                MessageBox.Show($"Your account was not verified and flagged accordingly. please refer to an administrator");
                disconnect_Click(null, null);
                return;
            }
            KeyValuePair<int, int> seat= GetWagon(id, departure, arrival, time);
            if (seat.Key == 0)
            {
                MessageBox.Show($"There are no more seats available in this train");
                disconnect_Click(null, null);
                skip = true;
            }
            TrainEntities aux_context = new TrainEntities();
            if (skip == true) return;
            else
            {
                var cardWindow = new CardWindow();
                cardWindow.ShowDialog();

                var ticket = new Ticket
                {
                    Price = Price,
                    User_id = this.user.User_id,
                    Wagon_id = seat.Key,
                    DayAndTime = time,
                    DepartureStation_id = Convert.ToInt32((from s in aux_context.Stations
                                                           where s.Name == departure
                                                           select s.Station_id).First()),
                    ArrivalStation_id = Convert.ToInt32((from s in aux_context.Stations
                                                         where s.Name == arrival
                                                         select s.Station_id).First()),
                    NumberOfSeat = seat.Value

                };

                using (var transaction = aux_context.Database.BeginTransaction())
                {
                    try
                    {
                        aux_context.Tickets.Add(ticket);
                        aux_context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("The ticket failed!");
                    }
                }
                MessageBox.Show($"Ticket bought!!!");
                disconnect_Click(null, null);
            }

        }

        private void bookButton_Click(object sender, RoutedEventArgs e)
        {
            string departure = departureCombo.Text;
            string arrival = arrivalCombo.Text;

            
            var trainData = from t in context.Trains
                        join ls in context.LineStations
                        on t.Line_id equals ls.Line_id    //for trains ids

                        join s in context.Stations
                        on ls.Station_id equals s.Station_id  //for departure stations

                        join ls2 in context.LineStations
                        on t.Line_id equals ls2.Line_id

                        join s2 in context.Stations
                        on ls2.Station_id equals s2.Station_id

                        where ls2.ArrivalTime != null && ls.DepartureTime != null &&
                        ls.Distance < ls2.Distance && ls.Line_id == ls2.Line_id && s.Name == departure && s2.Name == arrival
                        select new
                        {
                            //ID = ls.LineStations_id,
                            ID = t.Train_id,
                            Departure = s.Name,
                            Arrival = s2.Name,
                            Time = ls.DepartureTime,
                            Distance = ls2.Distance,
                            Price = ls2.Distance * 0.3
                        };

            foreach (var t in trainData)
            {
                if(t.ID == Convert.ToInt32(trip_id_box.Text))
                {
                    BookTicket(t.ID, t.Departure, t.Arrival, calendar.SelectedDate, t.Price);
                }
            }
        }


        private void arrivalCombo_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            fillArrivalComboBox();

        }

        private void departureCombo_SelectionChanged(object sender, TextChangedEventArgs e)
        {
            fillArrivalComboBox();
        }

        private void arrivalCombo_SelectionChanged(object sender, TextChangedEventArgs e)
        {

            string departure = departureCombo.Text;
            string arrival = arrivalCombo.Text;

            if(arrival.Length>=1){

                var trainData = from t in context.Trains
                                join ls in context.LineStations
                                on t.Line_id equals ls.Line_id    //for trains ids

                                join s in context.Stations
                                on ls.Station_id equals s.Station_id  //for departure stations

                                join ls2 in context.LineStations
                                on t.Line_id equals ls2.Line_id

                                join s2 in context.Stations
                                on ls2.Station_id equals s2.Station_id

                                where ls2.ArrivalTime != null && ls.DepartureTime != null &&
                                 ls.Distance < ls2.Distance && ls.Line_id == ls2.Line_id && s.Name == departure && s2.Name == arrival
                                select new
                                {
                                    //ID = ls.LineStations_id,
                                    ID = t.Train_id,
                                    Departure = s.Name,
                                    Arrival = s2.Name,
                                    Time = ls.DepartureTime,
                                    Distance = ls2.Distance,
                                    Price = ls2.Distance * 0.3
                                };


                if (trainData.ToList().Count() != 0)
                {
                    trainGrid.ItemsSource = trainData.ToList();
                }
                else
                {
                    MessageBox.Show($"There is no such train!");
                    return;
                };

            }
        }
    }
}
