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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TavernSkeep
{
    public partial class tickets : Window
    {
        RestClient client = new RestClient("http://localhost:8080");
        List<LineaTicket> lineasticket = new List<LineaTicket>();
        public tickets()
        {
            InitializeComponent();
            this.Cursor = Cursors.None;
            Cursor cursor = new Cursor(Application.GetResourceStream(new Uri("cursors/sword.cur", UriKind.Relative)).Stream);
            this.Cursor = cursor;
            LoadTickets();
        }

        private void LoadTickets()
        {
            string id = "/ticket";

            var request = new RestRequest(id, Method.Get);
            var response = client.GetAsync(request);
            List<Ticket> t1 = new List<Ticket>();

            try
            {
                if (!response.Result.Content.Equals("null"))
                    t1 = JsonConvert.DeserializeObject<List<Ticket>>(response.Result.Content);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }

            t1.ForEach(t =>
            {
                AllTickets.Items.Add(new Ticket { Codigo = t.Codigo, Mesa = t.Mesa, Fecha = t.Fecha });
            });
        }

        private void MostrarTicket(List<LineaTicket> list)
        {
            if (list.Count < 1)
                return;

            ListTicket.Items.Clear();

            double preciototal = 0;
            list.ForEach(m =>
            {
                preciototal += m.PrecioTotal;
                ListTicket.Items.Add(new LineaTicket { Nombre = m.Nombre, Cantidad = 1, PrecioUnidad = m.PrecioUnidad, PrecioTotal = m.PrecioTotal });
            });

            PrecioTotalT.Text = preciototal.ToString();
        }

        private void BuscarTicket_Click(object sender, RoutedEventArgs e)
        {
            string id = "/ticket/" + CodigoTicket.Text;

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
            MostrarTicket(t1.Listaproductos);
        }

        private void AllTickets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView list1 = sender as ListView;
            Ticket t1 = list1.SelectedItem as Ticket;

            string id = "/ticket/" + t1.Codigo;

            var request = new RestRequest(id, Method.Get);
            var response = client.GetAsync(request);
            Ticket t2 = new Ticket();

            try
            {
                if (!response.Result.Content.Equals("null"))
                    t2 = JsonConvert.DeserializeObject<Ticket>(response.Result.Content);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }
            MostrarTicket(t2.Listaproductos);
        }

        private void AllTickets_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
