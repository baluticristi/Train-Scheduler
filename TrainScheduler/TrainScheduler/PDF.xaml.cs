using MaterialDesignThemes.Wpf;
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

    public partial class PDF : Window
    {
        private TrainEntities context = new TrainEntities();


        public PDF()
        {
            InitializeComponent();
        }

        public void CreatePDF(User user)
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

                              where user.User_id == t.User_id && t.DepartureStation_id == st1.Station_id && t.ArrivalStation_id == st2.Station_id

                              select new
                              {
                                  TicketID = t.Ticket_id,
                                  BuyerName = user.FirstName + " " + user.LastName,
                                  TrainNumber = tr.Train_id,
                                  WagonNumber = wag.Wagon_id,
                                  SeatNumber = t.NumberOfSeat,
                                  DepartureStation = st1.Name,
                                  ArrivalStation = st2.Name,
                                  DepartureTime = ls1.DepartureTime,
                                  ArrivalTime = ls2.ArrivalTime,
                                  Price = t.Price,
                                  TripDate = t.DayAndTime
                              };
            var aux = ticketsData.First();
            tichetNumber.Text = Convert.ToString(aux.TicketID);
            buyerName.Text = aux.BuyerName;
            trainNumber.Text = Convert.ToString(aux.TrainNumber);
            wagonNumber.Text = Convert.ToString(aux.WagonNumber);
            seatNumber.Text = Convert.ToString(aux.SeatNumber);
            departureStation.Text = Convert.ToString(aux.DepartureStation);
            departureTime.Text = Convert.ToString(aux.DepartureTime);
            arrivalStation.Text = Convert.ToString(aux.ArrivalStation);
            arrivalTime.Text = Convert.ToString(aux.ArrivalTime);
            priceBlock.Text = Convert.ToString(aux.Price);

            SendPDF();

        }

        public void SendPDF()
        {

        }

    }
}
