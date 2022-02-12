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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace TavernSkeep
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RestClient client = new RestClient("http://localhost:8080");
        //static Cursor sword = new Cursor(Application.GetResourceStream(new Uri("sword.cur")).Stream);
        public MainWindow()
        {
            InitializeComponent();
            this.Cursor = Cursors.None;
            Cursor cursor = new Cursor(Application.GetResourceStream(new Uri("cursors/sword.cur", UriKind.Relative)).Stream);
            this.Cursor = cursor;
            //Mouse.OverrideCursor = sword;
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

            var request = new RestRequest(id, Method.Get);
            var response = client.GetAsync(request);
            //JObject json = JObject.Parse(response.Result.Content);
            Empleado emp1 = new Empleado();
            try
            {
                if (!response.Result.Content.Equals("null"))
                    emp1 = JsonConvert.DeserializeObject<Empleado>(response.Result.Content);
            }catch(Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }
            

            if (dni.Text.Equals(null) || dni.Text.Equals("") || dni.Text.Contains(" "))
            {
                MessageBox.Show("Se requiere introducir el DNI del empleado.");

            }
            else if (contraseña1.Text.Equals(null) || contraseña1.Text.Equals("") || contraseña1.Text.Equals(" "))
            {
                MessageBox.Show("Se requiere introducir la contraseña del empleado.");

            }
            else if (response.Result.Content.Equals("null"))
            {
                MessageBox.Show("El DNI del empleado introducido es incorrecto o no existe.");

            }
            else if (!emp1.Contraseña.Equals(contraseña1.Text))
            {
                MessageBox.Show("Contraseña incorrecta.");
            }
            else if (emp1.Dni.Equals(dni.Text) && emp1.Contraseña.Equals(contraseña1.Text))
            {
                this.Hide();
                SkeepHub a = new SkeepHub();
                a.WindowState = this.WindowState;
                a.Show();
                this.Close();
            }
            /*
            MessageBox.Show(myDetails.dni);
            MessageBox.Show(response.IsCompleted.ToString());
            MessageBox.Show(response.Result.Content);
            MessageBox.Show(json.ToString());
            */
        }


    }
}
