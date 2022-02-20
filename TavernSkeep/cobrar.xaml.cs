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
    /// Lógica de interacción para cobrar.xaml
    /// </summary>
    public partial class cobrar : Window
    {

        public double Preciot;
        public cobrar(double preciot)
        {
            InitializeComponent();
            this.Cursor = Cursors.None;
            Cursor cursor = new Cursor(Application.GetResourceStream(new Uri("cursors/sword.cur", UriKind.Relative)).Stream);
            this.Cursor = cursor;
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
    }
}
