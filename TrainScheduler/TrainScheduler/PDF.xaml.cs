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
//using System.Windows.Shapes;

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.IO.Packaging;
using System.IO;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

using MimeKit;
using MailKit.Net.Smtp;
using MailKit;
using MailKit.Security;
using System.Reflection.Emit;

namespace TrainScheduler
{

    public partial class PDF : Window
    {
        private TrainEntities context = new TrainEntities();
        public User User { get; set; }

        public PDF()
        {
            InitializeComponent();
   
        }

        public void showTicket(User user, int wagonId)
        {
            
            this.User= user;
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

                              where user.User_id == t.User_id && t.DepartureStation_id == st1.Station_id && t.ArrivalStation_id == st2.Station_id && wag.Wagon_id == wagonId

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
            tichetNumber.Text = aux.TicketID.ToString();
            buyerName.Text = aux.BuyerName;
            trainNumber.Text = Convert.ToString(aux.TrainNumber);
            wagonNumber.Text = Convert.ToString(aux.WagonNumber);
            seatNumber.Text = Convert.ToString(aux.SeatNumber);
            departureStation.Text = Convert.ToString(aux.DepartureStation);
            departureTime.Text = Convert.ToString(aux.DepartureTime);
            arrivalStation.Text = Convert.ToString(aux.ArrivalStation);
            arrivalTime.Text = Convert.ToString(aux.ArrivalTime);
            priceBlock.Text = Convert.ToString(aux.Price);

            
            SendPDF(user);
        }


        private string createPdf(User user)
        {
            string fileName = "bilet_" + user.User_id.ToString() + ".pdf";


            MemoryStream lMemoryStream = new MemoryStream();
            Package package = Package.Open(lMemoryStream, FileMode.Create);
            XpsDocument doc = new XpsDocument(package);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);

            // This is your window
            this.Show();
            writer.Write(this);

            doc.Close();
            package.Close();

            // Convert 
            MemoryStream outStream = new MemoryStream();
            PdfSharp.Xps.XpsConverter.Convert(lMemoryStream, outStream, false);

            // Write pdf file
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            outStream.CopyTo(fileStream);

           // this.Visibility = Visibility.Hidden;

            // Clean up
            outStream.Flush();
            outStream.Close();
            fileStream.Flush();
            fileStream.Close();
            this.Close();
            return fileName;

        }
        private void SendPDF(User user)
        {
            string fileName = createPdf(user);

            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress(user.LastName.ToString(), "CiuCiu_Train@outlook.com"));
            message.To.Add(MailboxAddress.Parse(user.email.ToString()));
            message.Subject = "CIUCIU TICKET";
            message.Body = new TextPart("plain")
            {
                Text = "Your ticket"
            };

            var attachment = new MimePart("application", "pdf")
            {
                Content = new MimeContent(File.OpenRead(fileName)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName (fileName)
            };

            var multipart = new Multipart("mixed");
            multipart.Add(new TextPart("plain") { Text = "Your Ticket:" });
            multipart.Add(attachment);    
            message.Body = multipart;

            string emailAddress = "CiuCiu_Train@outlook.com";
            string pass = "Train123";

            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate(emailAddress, pass);
                client.Send(message);

                Console.WriteLine("Email Sent!.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                client.Disconnect(true);

                client.Dispose();

                //this.Close();

            }
        }
    }
}
