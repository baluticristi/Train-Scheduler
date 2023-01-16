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
    /// Interaction logic for MessageBoxWin.xaml
    /// </summary>
    public partial class MessageBoxWin : Window
    {
        public MessageBoxWin()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
