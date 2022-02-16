using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
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
        List<List<Producto>> catPag = new List<List<Producto>>();
        List<List<Producto>> prPag = new List<List<Producto>>();
        List<Producto> prList = new List<Producto>();
        List<Producto> catList = new List<Producto>();
        List<Producto> menuList = new List<Producto>();
        PrivateFontCollection pfc = new PrivateFontCollection();
        int CurrentCategoryPage = 0;
        int CurrentProductPage = 0;
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

            int row = 0;
            int column = 0;

            // Cargar categorias
            foreach (Producto p in catPag[0])
            {

                Viewbox v = new Viewbox();
                v.Stretch = Stretch.Fill;

                Button b = new Button();
                b.MouseEnter += b1_MouseEnter;
                b.MouseLeave += b1_MouseLeave;

                StackPanel s = new StackPanel();
                s.Orientation = Orientation.Vertical;


                Image i = new Image();
                i.Source = new BitmapImage(new Uri(p.Imagen, UriKind.RelativeOrAbsolute));
                i.Width = 300;
                i.Height = 300;

                Label l = new Label();

                l.FontWeight = FontWeights.Bold;
                l.FontSize = 25;
                l.BorderThickness = new Thickness(2);
                l.BorderBrush = Brushes.Black;
                l.Background = Brushes.LightGray;
                l.HorizontalAlignment = HorizontalAlignment.Center;
                l.Content = p.Nombre;


                v.Child = b;
                b.Content = s;
                s.Children.Add(i);
                s.Children.Add(l);

                gridCategorias.Children.Add(v);

                Grid.SetRow(v, row);
                Grid.SetColumn(v, column);

                column++;
                if (column > 2)
                {
                    column = 0;
                    row++;
                }
            }

            // Cargar productos
            row = 0;
            column = 0;
            foreach (Producto p in prPag[0])
            {

                Viewbox v = new Viewbox();
                v.Stretch = Stretch.Fill;

                Button b = new Button();
                b.MouseEnter += b1_MouseEnter;
                b.MouseLeave += b1_MouseLeave;

                StackPanel s = new StackPanel();
                s.Orientation = Orientation.Vertical;
               

                Image i = new Image();
                i.Source = new BitmapImage(new Uri(p.Imagen, UriKind.RelativeOrAbsolute));
                i.Width = 300;
                i.Height = 300;

                Label l = new Label();
                
                l.FontWeight = FontWeights.Bold;
                l.FontSize = 25;
                l.BorderThickness = new Thickness(2);
                l.BorderBrush = Brushes.Black;
                l.Background = Brushes.LightGray;
                l.HorizontalAlignment = HorizontalAlignment.Center;
                l.Content = p.Nombre;
               

                v.Child = b;
                b.Content = s;
                s.Children.Add(i);
                s.Children.Add(l);

                gridProductos.Children.Add(v);

                Grid.SetRow(v, row);
                Grid.SetColumn(v, column);

                column++;
                if (column > 2)
                {
                    column = 0;
                    row++;
                }
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
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }

            foreach (Producto p in prList)
            {

                if (p.Es_categoria)
                    catList.Add(p);
                else
                    menuList.Add(p);
            }

            List<Producto> listin = new List<Producto>();

            int i = 0;
            int total = 0;

            foreach (Producto p in menuList)
            {
                
                if (i <= 11)
                {
                    listin.Add(p);
                    i++;
                } else
                {
                    prPag.Add(listin);
                    listin = new List<Producto>();
                    listin.Add(p);
                    i = 0;
                }

                if (total + 1 == menuList.Count)
                {
                    prPag.Add(listin);
                    break;
                }
                total++;
            }

            listin = new List<Producto>();

            i = 0;
            total = 0;

            foreach (Producto p in catList)
            {

                if (i <= 11)
                {
                    listin.Add(p);
                    i++;
                }
                else
                {
                    catPag.Add(listin);
                    listin = new List<Producto>();
                    listin.Add(p);
                    i = 0;
                }

                if (total + 1 == catList.Count)
                {
                    catPag.Add(listin);
                    break;
                }
                total++;
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

        private void MoreProducts(object sender, RoutedEventArgs e)
        {
            if (CurrentProductPage + 1 > prPag.Count)
            {
                MessageBox.Show("Es la última vez que te lo digo. No hay más productos");
            } else
            {
                CurrentProductPage++;
                UpdateProductPage(CurrentProductPage);
            }
        }

        private void LessProducts(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateProductPage(int number)
        {
            gridProductos.Children.Clear();

            foreach (Producto p in prPag[CurrentProductPage])
            {
                int row = 0;
                int column = 0;

                Viewbox v = new Viewbox();
                v.Stretch = Stretch.Fill;

                Button b = new Button();
                b.MouseEnter += b1_MouseEnter;
                b.MouseLeave += b1_MouseLeave;

                StackPanel s = new StackPanel();
                s.Orientation = Orientation.Vertical;


                Image i = new Image();
                i.Source = new BitmapImage(new Uri(p.Imagen, UriKind.RelativeOrAbsolute));
                i.Width = 300;
                i.Height = 300;

                Label l = new Label();

                l.FontWeight = FontWeights.Bold;
                l.FontSize = 25;
                l.BorderThickness = new Thickness(2);
                l.BorderBrush = Brushes.Black;
                l.Background = Brushes.LightGray;
                l.HorizontalAlignment = HorizontalAlignment.Center;
                l.Content = p.Nombre;


                v.Child = b;
                b.Content = s;
                s.Children.Add(i);
                s.Children.Add(l);

                gridProductos.Children.Add(v);

                Grid.SetRow(v, row);
                Grid.SetColumn(v, column);

                column++;
                if (column > 2)
                {
                    column = 0;
                    row++;
                }
            }
        }
    }
}
