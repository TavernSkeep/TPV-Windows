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
    /// <summary>
    /// Lógica de interacción para descuento.xaml
    /// </summary>
    public partial class descuento : Window
    {
        public descuento()
        {
            InitializeComponent();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
