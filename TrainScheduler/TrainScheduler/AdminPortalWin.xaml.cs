﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for AdminPortalWin.xaml
    /// </summary>
    public partial class AdminPortalWin : Window
    {
        private User admin;
        private TrainEntities context = new TrainEntities();
        public AdminPortalWin(User admin)
        {
            this.admin = admin;
            InitializeComponent();
        }

       public void ShowAdminPort()
       {
            this.acceptBtn.Visibility = Visibility.Hidden;
            this.declineBtn.Visibility = Visibility.Hidden;
            this.requestTxTBoX.Visibility = Visibility.Hidden;
            this.reqLabel.Visibility = Visibility.Hidden; 

            this.Show();
       }
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();

            window.ShowMainWin(this.admin);

            window.Show();

            this.Close();

        }

        private void requestBtn_Click(object sender, RoutedEventArgs e)
        {


            var data = from r in context.Requests
                       join u in context.Users
                       on r.User_id equals u.User_id
                       join rol in context.Roles
                       on u.Role_id equals rol.Role_id 
                       select new
                       {
                           r.Request_id,
                           u.LastName,
                           u.FirstName,
                           rol.Role_name,
                           r.date
                       };

            var listRequest = data.ToList();

            if(data != null)
            {
                this.acceptBtn.Visibility = Visibility.Visible;
                this.declineBtn.Visibility = Visibility.Visible;
                this.requestTxTBoX.Visibility = Visibility.Visible;
                this.reqLabel.Visibility = Visibility.Visible;

                requestGrid.ItemsSource = listRequest;
            }

        }

        int getUserId()
        {
            int id = Convert.ToInt32(requestTxTBoX.Text);  //id reqeust

            var data = from r in context.Requests
                       join u in context.Users
                       on r.User_id equals u.User_id
                       where r.Request_id == id
                       select new
                       {
                           u.User_id
                       };

            if (data == null)
                return -1;

            return data.ToList().First().User_id;

        }

        private void deleteRequest(int req_id)
        {
            Request req = context.Requests.Where(r => r.Request_id == req_id).First();
            context.Requests.Remove(req);
            context.SaveChanges();
        }
        private void acceptBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(requestTxTBoX.Text);  //id reqeust
            int u_id = getUserId();

            if (u_id == -1)
                return;

            User user = context.Users.Where(u => u.User_id == u_id).First();
            user.is_verified = true;
            context.SaveChanges();
            deleteRequest(id);
            MessageBox.Show("User was accepted!");
        }

        private void declineBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(requestTxTBoX.Text);  //id reqeust
            int u_id = getUserId();

            if (u_id == -1)
                return;

            User user = context.Users.Where(u => u.User_id == u_id).First();
            
            user.is_verified = true;
            user.Role_id = context.Roles.Where(r => r.Role_name == "Thief").First().Role_id;
            context.SaveChanges();
            deleteRequest(id);
            MessageBox.Show("User was not accepted!");
        }
    }
}
