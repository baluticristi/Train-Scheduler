using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Interaction logic for TripPlanner.xaml
    /// </summary>
    public partial class TripPlanner : Window
    {
        private User user;
        private TrainEntities context = new TrainEntities();
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
        public void showTripPlanWin(User user)
        {
            this.user = user;
            this.Show();
            fillDepartureComboBox();
        }


        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();

            window.ShowMainWin(this.user);

            this.Close();

        }
        private void BookTicket(int id, string departure, string arrival,DateTime? time, double? Price) {
            var ticket = new Ticket
            {
                Ticket_id = 1000,
                Price = Price,
                User_id = this.user.User_id,
                Wagon_id = null,
                DayAndTime = time,
                DepartureStation_id = Convert.ToInt32((from s in context.Stations
                                                       where s.Name == departure
                                                       select s.Station_id).First()),
                ArrivalStation_id = Convert.ToInt32((from s in context.Stations
                                                     where s.Name == arrival
                                                     select s.Station_id).First()),
                NumberOfSeat = null

            };
            ////////////////////////fix transaction
/*            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Tickets.Add(ticket);
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("The ticket failed!");
                }
            }
*/            MessageBox.Show($"Ticket bought!!!");
            disconnect_Click(null,null);

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
                            ID = ls.LineStations_id,
                            Departure = s.Name,
                            Arrival = s2.Name,
                            Time = ls.DepartureTime,
                            Distance = ls2.Distance,
                            Price = ls2.Distance * 1.7
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
                                    ID = ls.LineStations_id,
                                    Departure = s.Name,
                                    Arrival = s2.Name,
                                    Time = ls.DepartureTime,
                                    Distance = ls2.Distance,
                                    Price = ls2.Distance * 1.7
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
