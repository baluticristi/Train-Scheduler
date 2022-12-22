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

            var trainData = (from t in context.Trains
                             join ls in context.LineStations
                             on t.Line_id equals ls.Line_id    //for trains ids

                             join s in context.Stations
                             on ls.Station_id equals s.Station_id  //for departure stations

                             join ls2 in context.LineStations
                             on ls.Line_id equals ls2.Line_id

                             join s2 in context.Stations
                             on ls2.Station_id equals s2.Station_id

                             where ls2.ArrivalTime != null && ls.DepartureTime != null  && 
                             ls.Distance < ls2.Distance && ls.Line_id == ls2.Line_id && s.Name == departure_st

                             select new
                             {
                                 ArrivalStation = s2.Name
                             }).Distinct();                           //NU MERGE DISTINCT?

            foreach (var item in trainData.ToList())
            {
                arrivalCombo.Items.Add(item.ArrivalStation);
            }
        }
        public void showTripPlanWin(User user)
        {
            this.user = user;
            this.Show();
            fillDepartureComboBox();
        }
        private void showPossibleTrains()
        {
            //string depart_st = departureTxTBox.Text;
            //string arrival_st = arrivalTxTBox.Text;

            //var trainData = from t in context.Trains
            //                join ls in context.LineStations
            //                on t.Line_id equals ls.Line_id    //for trains ids

            //                join s in context.Stations
            //                on ls.Station_id equals s.Station_id  //for departure stations

            //                join ls2 in context.LineStations
            //                on t.Line_id equals ls2.Line_id

            //                join s2 in context.Stations
            //                on ls2.Station_id equals s2.Station_id

            //                //where s.dep
            //                select new
            //                {
            //                    t.Train_id,
            //                    DepartureStation = s.Name,
            //                    ArrivalStation = s2.Name,
            //                    DepartureTime = ls.DepartureTime

            //                };
        }
        private void showTrainsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void departureCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillArrivalComboBox();
        }
    }
}
