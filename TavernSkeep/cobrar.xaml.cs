using Newtonsoft.Json;
using RestSharp;
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
    public partial class cobrar : Window
    {
        RestClient client = new RestClient("http://localhost:8080");
        bool cobrarsatisfactorio = false;
        public bool CobrarSatisfactorio
        {
            get { return cobrarsatisfactorio; }
        }

        public double Preciot;
        Ticket tick;
        public cobrar(string preciot, Ticket ticket)
        {
            InitializeComponent();
            this.Cursor = Cursors.None;
            Cursor cursor = new Cursor(Application.GetResourceStream(new Uri("cursors/sword.cur", UriKind.Relative)).Stream);
            this.Cursor = cursor;
            Preciot = Convert.ToDouble(preciot);
            tick = ticket;
        }

        private void PagarTicket()
        {
            string id = "/ticket/" + tick;

            var request = new RestRequest(id, Method.Get);
            var response = client.GetAsync(request);
            Ticket t1 = new Ticket();

            try
            {
                if (!response.Result.Content.Equals("null"))
                    t1 = JsonConvert.DeserializeObject<Ticket>(response.Result.Content);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dinerorecibido.Text.Length > 15)
                return;

            Button b1 = sender as Button;
            dinerorecibido.Text += b1.Content;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            dinerorecibido.Text = "";
        }

        private void Tarjeta_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("A casa champion");
            Cobrar_Ticket();
            Close();
        }

        private void Cobrar_Ticket()
        {

            string id = "/mesa/" + tick.Mesa;

            var request = new RestRequest(id, Method.Get);
            var response = client.GetAsync(request);
            Mesa m = new Mesa();

            try
            {
                if (!response.Result.Content.Equals("null"))
                {
                    m = JsonConvert.DeserializeObject<Mesa>(response.Result.Content);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }

            // Método para modificar la mesa y añadirle el ticket actual

            request = new RestRequest("mesa/" + tick.Codigo, Method.Put);
            request.AddJsonBody(new { ticket_actual = "", zona = m.Zona, n_sillas = m.N_sillas, is_reservada = m.Is_reservada, codigo = m.Codigo });
            response = client.ExecutePutAsync(request);

            cobrarsatisfactorio = true;
        }

        private void Efectivo_Click(object sender, RoutedEventArgs e)
        {

            if (dinerorecibido.Text.Equals(""))
                return;

            double dinerodouble = double.Parse(dinerorecibido.Text);
            double devolucion = dinerodouble - Preciot;

            if (devolucion >= 0)
            {
                preciofinal.Text = "Devolucion:" + devolucion + "€";
                MessageBox.Show("Ticket cobrado con exito");
                Cobrar_Ticket();
                Close();
            }
            else
            {
                preciofinal.Text = "Falta dinero";
                dinerorecibido.Text = "";
            }


        }
    }
}