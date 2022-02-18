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

namespace TavernSkeep
{
    public partial class descuento : Window
    {

        public int descuentito = 0;
        public double Preciot;
        public double totaldescuento
        {
            get { return descuentito; }
        }

        public descuento(double preciot)
        {
            InitializeComponent();
            Preciot = preciot;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (descuentocalc.Text.Length > 9)
            {
                return;
            }
            Button b1 = sender as Button;
            descuentocalc.Text += b1.Content;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            descuentocalc.Text = "";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {

            if (descuentocalc.Text.Equals("123456789") && Preciot > 10)
                descuentito = 5;
            else if (descuentocalc.Text.Equals("987654321") && Preciot > 40)
                descuentito = 10;
            else if (descuentocalc.Text.Equals("000000000") && Preciot > 80)
                descuentito = 20;
            else
                MessageBox.Show("El código introducido no se puede aplicar");

            Close();
        }
    }
}
