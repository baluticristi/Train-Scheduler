using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for CardWindow.xaml
    /// </summary>
    public partial class CardWindow : Window
    {
        public bool isDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        public CardWindow()
        {
            InitializeComponent();
        }

        private void cancelApp(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private bool isOkFormatCardNumber()
        {
            int count = 0;
            foreach (char c in cardNumberBox.Text)
            {
                count++;
                if (count % 5 != 0)
                {
                    if (char.IsDigit(c) == false)
                        return false;
                }
            }
            return true;
        }

        public static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
        private bool isValidCardNumber()
        {
            string cardNumber = cardNumberBox.Text;
            cardNumber = RemoveWhitespace(cardNumber);

            int i, checkSum = 0;
            for(i = cardNumber.Length-1;i >= 0; i-=2) 
            {
                checkSum += (cardNumber[i] - '0');
            }

            for(i=cardNumber.Length-2;i>=0;i-=2)
            {
                int val = ((cardNumber[i] - '0') * 2);
                while (val > 0)
                {
                    checkSum += (val % 10);
                    val /= 10;
                }
            }

            return ((checkSum % 10) == 0);
            
        }
        private void payButton_Click(object sender, RoutedEventArgs e)
        {
            bool isNumeric;

            if (cardNumberBox.Text.Length < 19 || isOkFormatCardNumber() == false || isValidCardNumber() == false)
            {
                MessageBox.Show("Card Number is not corect!");
                return;
            }

            isNumeric = Regex.IsMatch(monthDate.Text, @"^\d+$");
            if(isNumeric == false || monthDate.Text.Length < 2 || Convert.ToInt32(monthDate.Text) > 12)
            {
                MessageBox.Show("Month is invalid!");
                return;
            }

            isNumeric = Regex.IsMatch(yearDate.Text, @"^\d+$");
            if (isNumeric == false || yearDate.Text.Length < 4 || Convert.ToInt32(yearDate.Text) > (DateTime.Now.Year + 7))
            {
                MessageBox.Show("Year is invalid!");
                return;
            }

            DateTime date = new DateTime(Convert.ToInt32(yearDate.Text), Convert.ToInt32(monthDate.Text), 1);
            
            ///////////////////////////////////////////////////
            ///TREBUIE VERIFICAT SI CVV
            ///
            if(cvvNumber.Text.Length < 3)
            {
                MessageBox.Show("The cvv is invalid");
                return;
            }

            if(date <= DateTime.Now)
            {
                MessageBox.Show("The card is expired");
                return;
            }

            MessageBox.Show("Succes!");
            this.Close();
        }

        private void CardNumberBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(e.Text);

            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;

            // Don't do anything if the text is already at the maximum length
            if (text.Length == 19)
            {
                return;
            }

            // Don't do anything if the user is trying to enter a space
            if (e.Text == " ")
            {
                return;
            }

            // Insert a space after the fourth character if necessary
            if ((text.Length + 1) % 5 == 0)
            {
                textBox.Text = text + " " + e.Text;
            }
            else
            {
                textBox.Text = text + e.Text;
            }
            textBox.SelectionStart = textBox.Text.Length;

            // Set the event's Handled property to true to prevent the character from being entered
            e.Handled = true;
        }

        private void monthDate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit((char)e.Text[0]))
            {
                e.Handled = true;
            }
        }

        private void yearDate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit((char)e.Text[0]))
            {
                e.Handled = true;
            }
        }

        private void cvvNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit((char)e.Text[0]))
            {
                e.Handled = true;
            }
        }
    }
}
