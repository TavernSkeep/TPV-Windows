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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestSharp;

namespace TavernSkeep
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("Always skeep = true");
            //Hola
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var res = new RestClient("http://10.2.40.119:8080");
            var request = new RestRequest("/empleado", Method.Post);
            res.ExecutePostAsync(request);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
            SkeepHub a = new SkeepHub();
            a.WindowState = this.WindowState;
            a.Show();
            this.Close();
        }
    }
}
