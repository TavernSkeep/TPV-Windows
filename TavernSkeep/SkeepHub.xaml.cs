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
using System.Windows.Threading;

namespace TavernSkeep
{
    /// <summary>
    /// Lógica de interacción para SkeepHub.xaml
    /// </summary>
    public partial class SkeepHub : Window
    {
        RestClient client = new RestClient("http://localhost:8080");
        List<Producto> prList = new List<Producto>();
        public SkeepHub()
        {
            InitializeComponent();
            this.Cursor = Cursors.None;
            Cursor cursor = new Cursor(Application.GetResourceStream(new Uri("cursors/sword.cur", UriKind.Relative)).Stream);
            this.Cursor = cursor;
            //media.Source = new Uri();
            Loading();
            startClock();
            ChargeProducts();
            ShowProducts();
        }

        private void ShowProducts()
        {
            List<Viewbox> viewboxes = new List<Viewbox>();

            foreach (Producto p in prList)
            {
                Viewbox v = new Viewbox();
                Button b = new Button();
                StackPanel s = new StackPanel();
                Image i = new Image();
                Label l = new Label();

                //v.Child(b);
                b.Content = s;
                s.Children.Add(i);
                s.Children.Add(l);
            }
        }

        private void ChargeProducts()
        {
            var request = new RestRequest("/producto", Method.Get);
            var response = client.GetAsync(request);
            //var lista = new List<Producto>();
            
            try
            {
                if (!response.Result.Content.Equals("null"))
                {
                    prList = JsonConvert.DeserializeObject<List<Producto>>(response.Result.Content);
                    MessageBox.Show(prList[0].Nombre);
                    //prList = lista;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }
        }

        DispatcherTimer timer = new DispatcherTimer();

        private void timer_tick(Object sender, EventArgs e)
        {
            timer.Stop();
            loadbg.Visibility = Visibility.Hidden;
            loaddots.Visibility = Visibility.Hidden;


            content.Visibility = Visibility.Visible;
        }

        void Loading()
        {
            timer.Tick += timer_tick;
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();
        }

        private void startClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {
            datetpv.Text = DateTime.Now.ToString("yyyy/MM/dd");
            timetpv.Text = DateTime.Now.ToString("HH:mm:ss");

            //("yyyy-MM-dd HH:mm:ss")
        }
        private void b1_MouseEnter(object sender, MouseEventArgs e)
        {
            Button enterPanel = sender as Button;
            StackPanel aux = enterPanel.Content as StackPanel;
            Label aux2 = aux.Children[1] as Label;
            aux2.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private void b1_MouseLeave(object sender, MouseEventArgs e)
        {
            Button enterPanel = sender as Button;
            StackPanel aux = enterPanel.Content as StackPanel;
            Label aux2 = aux.Children[1] as Label;
            aux2.Background = new SolidColorBrush(Colors.LightGray);
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            admin MiVentana = new admin();
            MiVentana.Owner = this;
            MiVentana.ShowDialog();
        }
    }
}
