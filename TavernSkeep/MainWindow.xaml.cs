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
using Newtonsoft.Json.Linq;
using RestSharp;

namespace TavernSkeep
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RestClient client = new RestClient("http://25.81.98.245:8080");
        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("Always skeep = true");
            //Hola
        }

        /*
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest("/empleado", Method.Post);
            res.ExecutePostAsync(request);
        }*/

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string id = "/empleado/" + dni.Text;
            
            try
            {
                var request = new RestRequest(id, Method.Get);
                var response = client.GetAsync(request);
                JObject json = JObject.Parse(response.Result.Content);
                MessageBox.Show(response.IsCompleted.ToString());
                MessageBox.Show(response.Result.Content);
                MessageBox.Show(json.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            this.Hide();
            SkeepHub a = new SkeepHub();
            a.WindowState = this.WindowState;
            a.Show();
            this.Close();
        }
    }
}
