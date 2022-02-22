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
    public partial class mesas : Window
    {
        public Ticket ticketmesa = new Ticket();
        public Ticket TicketVuelta
        {
            get { return ticketmesa; }
        }
        RestClient client = new RestClient("http://localhost:8080");
        ListView ListTicket;
        List<Button> BotonesMesas = new List<Button>();
        List<Mesa> ObjetosMesa = new List<Mesa>();
        List<Ticket> TicketsMesa = new List<Ticket>();
        public mesas(ListView listticket)
        {
            InitializeComponent();
            ListTicket = listticket;
            this.Cursor = Cursors.None;
            Cursor cursor = new Cursor(Application.GetResourceStream(new Uri("cursors/sword.cur", UriKind.Relative)).Stream);
            this.Cursor = cursor;
            LoadMesas();
        }

        private void LoadMesas()
        {
            string id = "/mesa";

            var request = new RestRequest(id, Method.Get);
            var response = client.GetAsync(request);

            try
            {
                if (!response.Result.Content.Equals("null"))
                {
                    ObjetosMesa = JsonConvert.DeserializeObject<List<Mesa>>(response.Result.Content);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }

            List<Mesa> ObjetosMesas = ObjetosMesa.OrderBy(m => m.Codigo).ToList();
            int fila = 0;
            int columna = 0;

            ObjetosMesas.ForEach(m =>
            {
                response = client.GetAsync(request);

                Viewbox v1 = new Viewbox();
                DockPanel sp = new DockPanel();

                sp.MinHeight = 180;
                sp.MinWidth = 450;


                v1.MouseLeftButtonDown += mesas_Click;
                v1.MouseRightButtonDown += mesas_Reservar;
                v1.Child = sp;
                v1.Tag = m;

                sp.HorizontalAlignment = HorizontalAlignment.Center;

               ImageBrush b6 = new ImageBrush();
               b6.ImageSource = new BitmapImage( new Uri("./../../images/boton.png", UriKind.Relative));
               sp.Background = b6;

                Image i1 = new Image();
                Label l1 = new Label();
                Label l2 = new Label();


                l1.FontSize = 30;
                l2.FontSize = 20;
                l1.Foreground = Brushes.White; 
                l2.Foreground = Brushes.White;


                l1.Content = m.Codigo;
                l2.Content = "Sillas:"+m.N_sillas;

                
                l1.VerticalAlignment = VerticalAlignment.Center;
                l1.HorizontalAlignment = HorizontalAlignment.Right; 
                l2.VerticalAlignment = VerticalAlignment.Center; 
                l2.HorizontalAlignment = HorizontalAlignment.Right;

                l1.IsEnabled = false;

                Thickness t1 = l1.Margin;
                t1.Right = 100;
                l1.Margin = t1;

                Thickness t2 = l2.Margin;
                t2.Right = 95;
                l2.Margin = t2;


                StackPanel sp1 = new StackPanel(); 
                sp1.VerticalAlignment = VerticalAlignment.Center;
                sp1.HorizontalAlignment = HorizontalAlignment.Center;

                sp1.Orientation = Orientation.Vertical;


                sp.Children.Add(i1);
                sp.Children.Add(sp1);
                sp1.Children.Add(l1);
                sp1.Children.Add(l2);




                if (m.Ticket_actual.Equals("uwu"))
                {

                    BitmapImage b2 = new BitmapImage();
                    b2.BeginInit();
                    b2.UriSource = new Uri("./../../images/s1.png", UriKind.Relative);
                    b2.EndInit();
                    i1.Source = b2;
                    ImageBrush b7 = new ImageBrush();
                    b7.ImageSource = new BitmapImage(new Uri("./../../images/boton_focus.png", UriKind.Relative));
                    sp.Background = b7;
                }
                else { 
                    BitmapImage b5 = new BitmapImage();
                    b5.BeginInit();
                    b5.UriSource = new Uri("./../../images/s2.png", UriKind.Relative);
                    b5.EndInit();
                    i1.Source = b5;
                    ImageBrush b8 = new ImageBrush();
                    b8.ImageSource = new BitmapImage(new Uri("./../../images/boton3.png", UriKind.Relative));
                    sp.Background = b8;
                }
                if (m.Is_reservada && m.Ticket_actual.Equals("uwu")) { 
                    BitmapImage b4 = new BitmapImage();
                    b4.BeginInit();
                    b4.UriSource = new Uri("./../../images/swords.png", UriKind.Relative);
                    b4.EndInit();
                    i1.Source = b4;
                    ImageBrush b9 = new ImageBrush();
                    b9.ImageSource = new BitmapImage(new Uri("./../../images/boton2_focus.png", UriKind.Relative));
                    sp.Background = b9;
                }

                GridBotones.Children.Add(v1);
                Grid.SetColumn(v1, columna);
                Grid.SetRow(v1, fila);

                columna++;

                if (columna > 2)
                {
                    fila++;
                    columna = 0;

                    RowDefinition rd = new RowDefinition();
                    GridBotones.RowDefinitions.Add(rd);
                }
            });


        }

        private void mesas_Click(object sender, MouseButtonEventArgs e)
        {
            Viewbox buttonmesa = sender as Viewbox;
            Mesa m = buttonmesa.Tag as Mesa;

            if (m.Ticket_actual.Equals("uwu") && ListTicket.Items.Count > 0)
            {
                Random rand = new Random();
                Ticket tick = new Ticket();
                tick.Id = "T" + rand.Next();
                tick.Codigo = tick.Id;
                tick.Mesa = m.Codigo;
                
                foreach (LineaTicket lt in ListTicket.Items) {
                    tick.Listaproductos.Add(lt);
                }

                // Método para subir el ticket a la bbdd

                var request = new RestRequest("ticket", Method.Post);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddJsonBody(tick);
                var response = client.ExecutePostAsync(request);

                // Método para modificar la mesa y añadirle el ticket actual

                request = new RestRequest("mesa/" + m.Codigo, Method.Put);
                request.AddJsonBody(new { ticket_actual = tick.Codigo, zona = m.Zona, n_sillas = m.N_sillas, is_reservada = m.Is_reservada, codigo = m.Codigo });
                response = client.ExecutePutAsync(request);

                Close();
            }

            else
            {

                // Método para coger el ticket actual de la mesa y devolverlo a la pantalla principal

                string id = "/ticket/" + m.Ticket_actual;

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

                ticketmesa = t1;

                Close();
            }

        }

        //Para reservar mesas o quitar la reserva de la mesa
        private void mesas_Reservar(object sender, MouseButtonEventArgs e)
        {
            Viewbox buttonmesa = sender as Viewbox;
            Mesa m = buttonmesa.Tag as Mesa;

            if (m.Is_reservada)
            {
                 var request = new RestRequest("mesa/" + m.Codigo, Method.Put);
                request.AddJsonBody(new { ticket_actual = m.Ticket_actual, zona = m.Zona, n_sillas = m.N_sillas, is_reservada = false, codigo = m.Codigo });
                var response = client.ExecutePutAsync(request);


                Close();
            }else
            {
                var request = new RestRequest("mesa/" + m.Codigo, Method.Put);
                request.AddJsonBody(new { ticket_actual = m.Ticket_actual, zona = m.Zona, n_sillas = m.N_sillas, is_reservada = true, codigo = m.Codigo });
                var response = client.ExecutePutAsync(request);

                Close();
            }
        }
    }
}