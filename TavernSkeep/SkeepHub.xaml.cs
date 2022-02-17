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
    public partial class SkeepHub : Window
    {
        RestClient client = new RestClient("http://localhost:8080");
        List<List<Producto>> catPag = new List<List<Producto>>();
        List<List<Producto>> prPag = new List<List<Producto>>();
        List<List<Producto>> productsFromCategory = new List<List<Producto>>();
        List<Producto> prList = new List<Producto>();
        List<Producto> catList = new List<Producto>();
        List<Producto> menuList = new List<Producto>();
        int CurrentCategoryPage = 0;
        int CurrentProductPage = 0;
        public SkeepHub(Empleado emp)
        {
            InitializeComponent();
            nametpv.Text = emp.Nombre;
            puestotpv.Text = emp.Puesto;
            this.Cursor = Cursors.None;
            Cursor cursor = new Cursor(Application.GetResourceStream(new Uri("cursors/sword.cur", UriKind.Relative)).Stream);
            this.Cursor = cursor;
            //media.Source = new Uri();
            Loading();
            startClock();
            ChargeProducts();
            UpdateCategoryPage(catPag, CurrentCategoryPage);
            UpdateProductPage(prPag, CurrentProductPage);
        }

        private void ChargeProducts()
        {
            var request = new RestRequest("/producto", Method.Get);
            var response = client.GetAsync(request);
            
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

        private void MoreCategories(object sender, RoutedEventArgs e)
        {
            if (CurrentProductPage >= catPag.Count - 1)
                return;
            else
            {
                CurrentCategoryPage++;
                UpdateCategoryPage(catPag, CurrentCategoryPage);
            }
        }

        private void LessCategories(object sender, RoutedEventArgs e)
        {
            if (CurrentCategoryPage < 1)
                return;
            else
            {
                CurrentCategoryPage--;
                UpdateCategoryPage(catPag, CurrentCategoryPage);
            }
        }

        private void MoreProducts(object sender, RoutedEventArgs e)
        {
            if (CurrentProductPage  >= prPag.Count - 1)
                return;
            else
            {
                CurrentProductPage++;
                UpdateProductPage(prPag, CurrentProductPage);
            }
        }

        private void LessProducts(object sender, RoutedEventArgs e)
        {
            if (CurrentProductPage < 1)
                return;
            else
            {
                CurrentProductPage--;
                UpdateProductPage(prPag, CurrentProductPage);
            }
        }

        private void UpdateCategoryPage(List<List<Producto>> list, int number)
        {
            gridProductos.Children.Clear();

            int row = 0;
            int column = 0;
            foreach (Producto p in list[number])
            {

                Viewbox v = new Viewbox();
                v.Stretch = Stretch.Fill;

                Button b = new Button();

                if (p.Nombre.Equals("Todo"))
                    b.Click += Todo_Click;
                else
                    b.Click += Categoria_Click;

                b.MouseEnter += b1_MouseEnter;
                b.MouseLeave += b1_MouseLeave;
                b.Tag = p;

                b.BorderThickness = new Thickness(2);
                b.BorderBrush = Brushes.White;
                b.Background = Brushes.Transparent;

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
        }

        private void UpdateProductPage(List<List<Producto>> list, int number)
        {
            gridProductos.Children.Clear();

            if (list.Count < 1)
                return;

            int row = 0;
            int column = 0;

            foreach (Producto p in list[number])
            {

                Viewbox v = new Viewbox();
                v.Stretch = Stretch.Fill;

                Button b = new Button();
                b.Tag = p;
                b.MouseEnter += b1_MouseEnter;
                b.MouseLeave += b1_MouseLeave;
                b.Click += Product_Click;
                b.MouseRightButtonDown += Product_RightClick;

                b.BorderThickness = new Thickness(2);
                b.BorderBrush = Brushes.White;
                b.Background = Brushes.Transparent;

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

        private void Categoria_Click(object sender, RoutedEventArgs e)
        {
            productsFromCategory.Clear();

            Button boton = sender as Button;
            Producto c = boton.Tag as Producto;

            List<Producto> catProducts = new List<Producto>();

            int i = 0;
            int total = 0;

            foreach (Producto p in menuList)
            {
                if (p.Tipo_producto.Equals(c.Nombre))
                {
                    catProducts.Add(p);
                }
                else
                    continue;
            }

            List<Producto> listin = new List<Producto>();

            foreach (Producto p in catProducts)
            {
                if (i <= 11)
                {
                    listin.Add(p);

                    i++;
                }
                else
                {
                    productsFromCategory.Add(listin);
                    listin = new List<Producto>();
                    listin.Add(p);
                    i = 0;
                }

                if (total + 1 == catProducts.Count)
                {
                    productsFromCategory.Add(listin);
                    break;
                }
                total++;
            }

            CurrentProductPage = 0;
            UpdateProductPage(productsFromCategory, CurrentProductPage);
        }

        private void Todo_Click(object sender, RoutedEventArgs e)
        {
            CurrentProductPage = 0;
            UpdateProductPage(prPag, CurrentProductPage);
        }

        private void Product_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Amogus Click");
        }

        private void Product_RightClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            ProductPreview preview = new ProductPreview(b.Tag as Producto);
            preview.ShowDialog();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            mesas MesasVentana = new mesas();
            MesasVentana.Owner = this;
            MesasVentana.ShowDialog();
            
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            tickets ticketsVentana = new tickets();
            ticketsVentana.Owner = this;
            ticketsVentana.ShowDialog();
        }
    }
}
