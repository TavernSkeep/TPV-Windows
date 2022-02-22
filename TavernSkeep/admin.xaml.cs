using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class admin : Window
    {
        RestClient client = new RestClient("http://localhost:8080");
        TextBox nombre;
        TextBox apellidos;
        TextBox dni;
        TextBox contrasena;
        TextBox email;
        TextBox telefono;
        TextBox puesto;
        TextBox specs;
        TextBox precio;
        TextBox imagen;
        TextBox stock;
        TextBox tipoprod;
        CheckBox es_categoria;
        public admin()
        {
            InitializeComponent();
            this.Cursor = Cursors.None;
            Cursor cursor = new Cursor(Application.GetResourceStream(new Uri("cursors/sword.cur", UriKind.Relative)).Stream);
            this.Cursor = cursor;
        }

        private void NuevoProducto(string Nombre, string Specs, Double Precio, string Imagen, int Stock, string Tipo_producto, Boolean Es_categoria)
        {
            Producto pr1 = new Producto(Nombre, Specs, Precio, Imagen, Stock, Tipo_producto, Es_categoria);
            var request = new RestRequest("producto", Method.Post);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddJsonBody(pr1);
            var response = client.ExecutePostAsync(request);

            MessageBox.Show("Producto " + pr1.Nombre + " insertado correctamente");
            nombre.Text = "";
            specs.Text = "";
            precio.Text = "";
            imagen.Text = "";
            stock.Text = "";
            tipoprod.Text = "";
        }

        private void BorrarProducto(string Nombre)
        {
            var request = new RestRequest("producto/" + Nombre, Method.Delete);
            request.RequestFormat = RestSharp.DataFormat.Json;
            var response = client.ExecuteAsync(request);

            MessageBox.Show("Producto: [" + Nombre + "] eliminado con éxito");
            nombre.Text = "";
        }

        private void EditarProducto(string Specs, double DoublePrecio, int IntStock)
        {
            var request = new RestRequest("producto/" + nombre.Text, Method.Put);
            request.AddJsonBody(new { nombre = nombre.Text, especificaciones = Specs, precio = DoublePrecio, stock = IntStock });
            var response = client.ExecutePutAsync(request);

            MessageBox.Show("Producto: [" + nombre.Text + "] modificado satisfactoriamente.");
            nombre.Text = "";
            specs.Text = "";
            precio.Text = "";
            stock.Text = "";
        }

        private void EditarEmpleado(string Dni, string Contraseña, string Puesto, string Telefono, string Email)
        {
            var request = new RestRequest("empleado/" + dni, Method.Put);
            request.AddJsonBody(new { dni = Dni, contraseña = Contraseña, puesto = Puesto, telefono = Telefono, email = Email });
            var response = client.ExecutePutAsync(request);

            MessageBox.Show("Empleado con DNI: [" + Dni + "] modificado satisfactoriamente.");
            dni.Text = "";
            contrasena.Text = "";
            puesto.Text = "";
            telefono.Text = "";
            email.Text = "";
        }

        private void CrearEmpleado(string Nombre, string Apellidos, string Contrasena, string Dni, string Puesto, string Telefono, string Email)
        {

            Empleado emp1 = new Empleado(Nombre, Apellidos, Contrasena, Dni, Puesto, Telefono, Email);

            var request = new RestRequest("empleado", Method.Post);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddJsonBody(emp1);
            var response = client.ExecutePostAsync(request);

            MessageBox.Show("Empleado " + emp1.Nombre + " " + emp1.Apellidos + " creado satisfactoriamente");

            nombre.Text = "";
            apellidos.Text = "";
            dni.Text = "";
            contrasena.Text = "";
            email.Text = "";
            telefono.Text = "";
            puesto.Text = "";
        }

        private void BorrarEmpleado(string Dni)
        {
            var request = new RestRequest("empleado/" + Dni, Method.Delete);
            request.RequestFormat = RestSharp.DataFormat.Json;
            var response = client.ExecuteAsync(request);

            MessageBox.Show("Empleado con DNI: " + Dni + " eliminado con éxito");
            dni.Text = "";
        }

        private void BorrarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            GridDatos.Children.Clear();
            GridDatos.RowDefinitions.Clear();

            // Eliminar empleado

            Label CrearEmp = new Label();
            CrearEmp.Content = "+Borrar Empleado+";
            CrearEmp.Foreground = Brushes.Gold;
            CrearEmp.FontSize = 20;
            CrearEmp.HorizontalAlignment = HorizontalAlignment.Center;
            CrearEmp.VerticalAlignment = VerticalAlignment.Center;

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(CrearEmp);
            Grid.SetRow(CrearEmp, 0);
            Grid.SetColumn(CrearEmp, 0);

            // DNI

            StackPanel st1 = new StackPanel();
            st1.Orientation = Orientation.Vertical;
            st1.Background = Brushes.Transparent;

            Label LblDni = new Label();
            LblDni.Content = "DNI:";
            LblDni.Foreground = Brushes.Gold;
            LblDni.FontSize = 20;
            LblDni.HorizontalAlignment = HorizontalAlignment.Center;
            LblDni.VerticalAlignment = VerticalAlignment.Center;

            dni = new TextBox();
            dni.Background = Brushes.Transparent;
            dni.TextAlignment = TextAlignment.Center;
            dni.Foreground = Brushes.Gold;
            dni.FontSize = 18;
            dni.MaxLength = 20;

            st1.Children.Add(LblDni);
            st1.Children.Add(dni);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st1);
            Grid.SetRow(st1, 1);
            Grid.SetColumn(st1, 0);


            StackPanel st2 = new StackPanel();
            st2.Orientation = Orientation.Vertical;
            st2.Background = Brushes.Transparent;

            Image b1 = new Image();
            b1.MouseLeftButtonDown += ButtonBorrar_Click;
            b1.MouseEnter += Boton_Hover;
            b1.MouseLeave += Boton_Leave;

            Label l1 = new Label();
            l1.Content = "/Eliminar/";
            l1.Foreground = Brushes.Gold;
            l1.FontSize = 20;
            l1.HorizontalAlignment = HorizontalAlignment.Center;
            l1.VerticalAlignment = VerticalAlignment.Center;


            BitmapImage b3 = new BitmapImage();
            b3.BeginInit();
            b3.UriSource = new Uri("./../../images/boton.png", UriKind.Relative);
            b3.EndInit();
            b1.Source = b3;
            b1.Width = 170;
            b1.Height = 80;

            st2.Children.Add(b1);
            st2.Children.Add(l1);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st2);
            Grid.SetRow(st2, 8);
            Grid.SetColumn(st2, 0);
        }

        private void CrearProducto_Click(object sender, RoutedEventArgs e)
        {
            GridDatos.Children.Clear();
            GridDatos.RowDefinitions.Clear();

            // Crear producto

            Label CrearPr = new Label();
            CrearPr.Content = "+Crear Producto+";
            CrearPr.Foreground = Brushes.Gold;
            CrearPr.FontSize = 20;
            CrearPr.HorizontalAlignment = HorizontalAlignment.Center;
            CrearPr.VerticalAlignment = VerticalAlignment.Center;

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(CrearPr);
            Grid.SetRow(CrearPr, 0);
            Grid.SetColumn(CrearPr, 0);


            // Nombre

            StackPanel st1 = new StackPanel();
            st1.Orientation = Orientation.Vertical;
            st1.Background = Brushes.Transparent;

            Label LblNombre = new Label();
            LblNombre.Content = "Nombre:";
            LblNombre.Foreground = Brushes.Gold;
            LblNombre.FontSize = 20;
            LblNombre.HorizontalAlignment = HorizontalAlignment.Center;
            LblNombre.VerticalAlignment = VerticalAlignment.Center;

            nombre = new TextBox();
            nombre.Background = Brushes.Transparent;
            nombre.TextAlignment = TextAlignment.Center;
            nombre.Foreground = Brushes.Gold;
            nombre.FontSize = 18;
            nombre.MaxLength = 20;

            st1.Children.Add(LblNombre);
            st1.Children.Add(nombre);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st1);
            Grid.SetRow(st1, 1);
            Grid.SetColumn(st1, 0);

            // Especificaciones

            StackPanel st2 = new StackPanel();
            st2.Orientation = Orientation.Vertical;
            st2.Background = Brushes.Transparent;

            Label LblSpecs = new Label();
            LblSpecs.Content = "Especificaciones:";
            LblSpecs.Foreground = Brushes.Gold;
            LblSpecs.FontSize = 20;
            LblSpecs.HorizontalAlignment = HorizontalAlignment.Center;
            LblSpecs.VerticalAlignment = VerticalAlignment.Center;

            specs = new TextBox();
            specs.Background = Brushes.Transparent;
            specs.TextAlignment = TextAlignment.Center;
            specs.Foreground = Brushes.Gold;
            specs.FontSize = 18;
            specs.MaxLength = 200;

            st2.Children.Add(LblSpecs);
            st2.Children.Add(specs);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st2);
            Grid.SetRow(st2, 2);
            Grid.SetColumn(st2, 0);

            // Precio

            StackPanel st3 = new StackPanel();
            st3.Orientation = Orientation.Vertical;
            st3.Background = Brushes.Transparent;

            Label LblPrecio = new Label();
            LblPrecio.Content = "Precio:";
            LblPrecio.Foreground = Brushes.Gold;
            LblPrecio.FontSize = 20;
            LblPrecio.HorizontalAlignment = HorizontalAlignment.Center;
            LblPrecio.VerticalAlignment = VerticalAlignment.Center;

            precio = new TextBox();
            precio.Background = Brushes.Transparent;
            precio.TextAlignment = TextAlignment.Center;
            precio.Foreground = Brushes.Gold;
            precio.FontSize = 18;
            precio.MaxLength = 20;

            st3.Children.Add(LblPrecio);
            st3.Children.Add(precio);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st3);
            Grid.SetRow(st3, 3);
            Grid.SetColumn(st3, 0);

            // Imagen

            StackPanel st4 = new StackPanel();
            st4.Orientation = Orientation.Vertical;
            st4.Background = Brushes.Transparent;

            Label LblImagen = new Label();
            LblImagen.Content = "Imagen (url):";
            LblImagen.Foreground = Brushes.Gold;
            LblImagen.FontSize = 20;
            LblImagen.HorizontalAlignment = HorizontalAlignment.Center;
            LblImagen.VerticalAlignment = VerticalAlignment.Center;

            imagen = new TextBox();
            imagen.Background = Brushes.Transparent;
            imagen.TextAlignment = TextAlignment.Center;
            imagen.Foreground = Brushes.Gold;
            imagen.FontSize = 18;
            imagen.MaxLength = 20;

            st4.Children.Add(LblImagen);
            st4.Children.Add(imagen);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st4);
            Grid.SetRow(st4, 4);
            Grid.SetColumn(st4, 0);

            // Stock

            StackPanel st5 = new StackPanel();
            st5.Orientation = Orientation.Vertical;
            st5.Background = Brushes.Transparent;

            Label LblStock = new Label();
            LblStock.Content = "Stock:";
            LblStock.Foreground = Brushes.Gold;
            LblStock.FontSize = 20;
            LblStock.HorizontalAlignment = HorizontalAlignment.Center;
            LblStock.VerticalAlignment = VerticalAlignment.Center;

            stock = new TextBox();
            stock.Background = Brushes.Transparent;
            stock.TextAlignment = TextAlignment.Center;
            stock.Foreground = Brushes.Gold;
            stock.FontSize = 18;
            stock.MaxLength = 20;
            stock.PreviewTextInput += TextboxOnlyInt;

            st5.Children.Add(LblStock);
            st5.Children.Add(stock);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st5);
            Grid.SetRow(st5, 5);
            Grid.SetColumn(st5, 0);

            // Tipo producto

            StackPanel st6 = new StackPanel();
            st6.Orientation = Orientation.Vertical;
            st6.Background = Brushes.Transparent;

            Label LblTipo = new Label();
            LblTipo.Content = "Tipo producto:";
            LblTipo.Foreground = Brushes.Gold;
            LblTipo.FontSize = 20;
            LblTipo.HorizontalAlignment = HorizontalAlignment.Center;
            LblTipo.VerticalAlignment = VerticalAlignment.Center;

            tipoprod = new TextBox();
            tipoprod.Background = Brushes.Transparent;
            tipoprod.TextAlignment = TextAlignment.Center;
            tipoprod.Foreground = Brushes.Gold;
            tipoprod.FontSize = 18;
            tipoprod.MaxLength = 20;

            st6.Children.Add(LblTipo);
            st6.Children.Add(tipoprod);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st6);
            Grid.SetRow(st6, 6);
            Grid.SetColumn(st6, 0);

            // ¿Es categoria?

            StackPanel st7 = new StackPanel();
            st7.Orientation = Orientation.Vertical;
            st7.Background = Brushes.Transparent;

            Label LblCategoria = new Label();
            LblCategoria.Content = "Marcar si es categoria";
            LblCategoria.Foreground = Brushes.Gold;
            LblCategoria.FontSize = 20;
            LblCategoria.HorizontalAlignment = HorizontalAlignment.Center;
            LblCategoria.VerticalAlignment = VerticalAlignment.Center;

            es_categoria = new CheckBox();
            es_categoria.Background = Brushes.Transparent;
            es_categoria.Foreground = Brushes.Gold;
            es_categoria.FontSize = 18;
            es_categoria.HorizontalAlignment = HorizontalAlignment.Center;

            st7.Children.Add(LblCategoria);
            st7.Children.Add(es_categoria);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st7);
            Grid.SetRow(st7, 7);
            Grid.SetColumn(st7, 0);

            StackPanel st8 = new StackPanel();
            st8.Orientation = Orientation.Vertical;
            st8.Background = Brushes.Transparent;

            Image b1 = new Image();
            b1.MouseLeftButtonDown += NuevoProducto_Click;
            b1.MouseEnter += Boton_Hover;
            b1.MouseLeave += Boton_Leave;

            Label l1 = new Label();
            l1.Content = "/Crear Producto/";
            l1.Foreground = Brushes.Gold;
            l1.FontSize = 20;
            l1.HorizontalAlignment = HorizontalAlignment.Center;
            l1.VerticalAlignment = VerticalAlignment.Center;


            BitmapImage b3 = new BitmapImage();
            b3.BeginInit();
            b3.UriSource = new Uri("./../../images/boton.png", UriKind.Relative);
            b3.EndInit();
            b1.Source = b3;
            b1.Width = 170;
            b1.Height = 80;

            st8.Children.Add(b1);
            st8.Children.Add(l1);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st8);
            Grid.SetRow(st8, 8);
            Grid.SetColumn(st8, 0);
        }

        private void CrearEmpleado_Click(object sender, RoutedEventArgs e)
        {
            GridDatos.Children.Clear();
            GridDatos.RowDefinitions.Clear();

            // Crear empleado

            Label CrearEmp = new Label();
            CrearEmp.Content = "+Crear Empleado+";
            CrearEmp.Foreground = Brushes.Gold;
            CrearEmp.FontSize = 20;
            CrearEmp.HorizontalAlignment = HorizontalAlignment.Center;
            CrearEmp.VerticalAlignment = VerticalAlignment.Center;

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(CrearEmp);
            Grid.SetRow(CrearEmp, 0);
            Grid.SetColumn(CrearEmp, 0);

            // Nombre

            StackPanel st1 = new StackPanel();
            st1.Orientation = Orientation.Vertical;
            st1.Background = Brushes.Transparent;

            Label LblNombre = new Label();
            LblNombre.Content = "Nombre:";
            LblNombre.Foreground = Brushes.Gold;
            LblNombre.FontSize = 20;
            LblNombre.HorizontalAlignment = HorizontalAlignment.Center;
            LblNombre.VerticalAlignment = VerticalAlignment.Center;

            nombre = new TextBox();
            nombre.Background = Brushes.Transparent;
            nombre.TextAlignment = TextAlignment.Center;
            nombre.Foreground = Brushes.Gold;
            nombre.FontSize = 18;
            nombre.MaxLength = 20;

            st1.Children.Add(LblNombre);
            st1.Children.Add(nombre);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st1);
            Grid.SetRow(st1, 1);
            Grid.SetColumn(st1, 0);

            // Apellidos

            StackPanel st2 = new StackPanel();
            st2.Orientation = Orientation.Vertical;
            st2.Background = Brushes.Transparent;

            Label LblApellidos = new Label();
            LblApellidos.Content = "Apellidos:";
            LblApellidos.Foreground = Brushes.Gold;
            LblApellidos.FontSize = 20;
            LblApellidos.HorizontalAlignment = HorizontalAlignment.Center;
            LblApellidos.VerticalAlignment = VerticalAlignment.Center;

            apellidos = new TextBox();
            apellidos.Background = Brushes.Transparent;
            apellidos.TextAlignment = TextAlignment.Center;
            apellidos.Foreground = Brushes.Gold;
            apellidos.FontSize = 18;
            apellidos.MaxLength = 20;

            st2.Children.Add(LblApellidos);
            st2.Children.Add(apellidos);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st2);
            Grid.SetRow(st2, 2);
            Grid.SetColumn(st2, 0);

            // DNI

            StackPanel st3 = new StackPanel();
            st3.Orientation = Orientation.Vertical;
            st3.Background = Brushes.Transparent;

            Label LblDni = new Label();
            LblDni.Content = "DNI:";
            LblDni.Foreground = Brushes.Gold;
            LblDni.FontSize = 20;
            LblDni.HorizontalAlignment = HorizontalAlignment.Center;
            LblDni.VerticalAlignment = VerticalAlignment.Center;

            dni = new TextBox();
            dni.Background = Brushes.Transparent;
            dni.TextAlignment = TextAlignment.Center;
            dni.Foreground = Brushes.Gold;
            dni.FontSize = 18;
            dni.MaxLength = 20;

            st3.Children.Add(LblDni);
            st3.Children.Add(dni);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st3);
            Grid.SetRow(st3, 3);
            Grid.SetColumn(st3, 0);

            // Contraseña

            StackPanel st4 = new StackPanel();
            st4.Orientation = Orientation.Vertical;
            st4.Background = Brushes.Transparent;

            Label LblContrasena = new Label();
            LblContrasena.Content = "Contraseña:";
            LblContrasena.Foreground = Brushes.Gold;
            LblContrasena.FontSize = 20;
            LblContrasena.HorizontalAlignment = HorizontalAlignment.Center;
            LblContrasena.VerticalAlignment = VerticalAlignment.Center;

            contrasena = new TextBox();
            contrasena.Background = Brushes.Transparent;
            contrasena.TextAlignment = TextAlignment.Center;
            contrasena.Foreground = Brushes.Gold;
            contrasena.FontSize = 18;
            contrasena.MaxLength = 20;

            st4.Children.Add(LblContrasena);
            st4.Children.Add(contrasena);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st4);
            Grid.SetRow(st4, 4);
            Grid.SetColumn(st4, 0);

            // Puesto

            StackPanel st5 = new StackPanel();
            st5.Orientation = Orientation.Vertical;
            st5.Background = Brushes.Transparent;

            Label LblPuesto = new Label();
            LblPuesto.Content = "Puesto:";
            LblPuesto.Foreground = Brushes.Gold;
            LblPuesto.FontSize = 20;
            LblPuesto.HorizontalAlignment = HorizontalAlignment.Center;
            LblPuesto.VerticalAlignment = VerticalAlignment.Center;

            puesto = new TextBox();
            puesto.Background = Brushes.Transparent;
            puesto.TextAlignment = TextAlignment.Center;
            puesto.Foreground = Brushes.Gold;
            puesto.FontSize = 18;
            puesto.MaxLength = 20;

            st5.Children.Add(LblPuesto);
            st5.Children.Add(puesto);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st5);
            Grid.SetRow(st5, 5);
            Grid.SetColumn(st5, 0);

            // E-mail

            StackPanel st6 = new StackPanel();
            st6.Orientation = Orientation.Vertical;
            st6.Background = Brushes.Transparent;

            Label LblEmail = new Label();
            LblEmail.Content = "Email:";
            LblEmail.Foreground = Brushes.Gold;
            LblEmail.FontSize = 20;
            LblEmail.HorizontalAlignment = HorizontalAlignment.Center;
            LblEmail.VerticalAlignment = VerticalAlignment.Center;

            email = new TextBox();
            email.Background = Brushes.Transparent;
            email.TextAlignment = TextAlignment.Center;
            email.Foreground = Brushes.Gold;
            email.FontSize = 18;
            email.MaxLength = 20;

            st6.Children.Add(LblEmail);
            st6.Children.Add(email);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st6);
            Grid.SetRow(st6, 6);
            Grid.SetColumn(st6, 0);

            // Teléfono

            StackPanel st7 = new StackPanel();
            st7.Orientation = Orientation.Vertical;
            st7.Background = Brushes.Transparent;

            Label LblTelefono = new Label();
            LblTelefono.Content = "Telefono:";
            LblTelefono.Foreground = Brushes.Gold;
            LblTelefono.FontSize = 20;
            LblTelefono.HorizontalAlignment = HorizontalAlignment.Center;
            LblTelefono.VerticalAlignment = VerticalAlignment.Center;

            telefono = new TextBox();
            telefono.Background = Brushes.Transparent;
            telefono.TextAlignment = TextAlignment.Center;
            telefono.Foreground = Brushes.Gold;
            telefono.FontSize = 18;
            telefono.MaxLength = 20;

            st7.Children.Add(LblTelefono);
            st7.Children.Add(telefono);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st7);
            Grid.SetRow(st7, 7);
            Grid.SetColumn(st7, 0);

            StackPanel st8 = new StackPanel();
            st8.Orientation = Orientation.Vertical;
            st8.Background = Brushes.Transparent;

            Image b1 = new Image();
            b1.MouseLeftButtonDown += CrearEmpleado_Click;
            b1.MouseEnter += Boton_Hover;
            b1.MouseLeave += Boton_Leave;

            Label l1 = new Label();
            l1.Content = "/Crear Empleado/";
            l1.Foreground = Brushes.Gold;
            l1.FontSize = 20;
            l1.HorizontalAlignment = HorizontalAlignment.Center;
            l1.VerticalAlignment = VerticalAlignment.Center;


            BitmapImage b3 = new BitmapImage();
            b3.BeginInit();
            b3.UriSource = new Uri("./../../images/boton.png", UriKind.Relative);
            b3.EndInit();
            b1.Source = b3;
            b1.Width = 170;
            b1.Height = 80;

            st8.Children.Add(b1);
            st8.Children.Add(l1);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st8);
            Grid.SetRow(st8, 8);
            Grid.SetColumn(st8, 0);
        }

        private void ModificarProducto_Click(object sender, RoutedEventArgs e)
        {
            GridDatos.Children.Clear();
            GridDatos.RowDefinitions.Clear();

            // Modificar producto

            Label ModificarProd = new Label();
            ModificarProd.Content = "+Modificar Producto+";
            ModificarProd.Foreground = Brushes.Gold;
            ModificarProd.FontSize = 20;
            ModificarProd.HorizontalAlignment = HorizontalAlignment.Center;
            ModificarProd.VerticalAlignment = VerticalAlignment.Center;

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(ModificarProd);
            Grid.SetRow(ModificarProd, 0);
            Grid.SetColumn(ModificarProd, 0);

            // Nombre

            StackPanel st1 = new StackPanel();
            st1.Orientation = Orientation.Vertical;
            st1.Background = Brushes.Transparent;

            Label LblNombre = new Label();
            LblNombre.Content = "Nombre:";
            LblNombre.Foreground = Brushes.Gold;
            LblNombre.FontSize = 20;
            LblNombre.HorizontalAlignment = HorizontalAlignment.Center;
            LblNombre.VerticalAlignment = VerticalAlignment.Center;

            nombre = new TextBox();
            nombre.Background = Brushes.Transparent;
            nombre.TextAlignment = TextAlignment.Center;
            nombre.Foreground = Brushes.Gold;
            nombre.FontSize = 18;
            nombre.MaxLength = 20;

            st1.Children.Add(LblNombre);
            st1.Children.Add(nombre);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st1);
            Grid.SetRow(st1, 1);
            Grid.SetColumn(st1, 0);

            // Especificaciones

            StackPanel st2 = new StackPanel();
            st2.Orientation = Orientation.Vertical;
            st2.Background = Brushes.Transparent;

            Label LblSpecs = new Label();
            LblSpecs.Content = "Especificaciones:";
            LblSpecs.Foreground = Brushes.Gold;
            LblSpecs.FontSize = 20;
            LblSpecs.HorizontalAlignment = HorizontalAlignment.Center;
            LblSpecs.VerticalAlignment = VerticalAlignment.Center;

            specs = new TextBox();
            specs.Background = Brushes.Transparent;
            specs.TextAlignment = TextAlignment.Center;
            specs.Foreground = Brushes.Gold;
            specs.FontSize = 18;
            specs.MaxLength = 200;

            st2.Children.Add(LblSpecs);
            st2.Children.Add(specs);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st2);
            Grid.SetRow(st2, 2);
            Grid.SetColumn(st2, 0);

            // Precio

            StackPanel st3 = new StackPanel();
            st2.Orientation = Orientation.Vertical;
            st2.Background = Brushes.Transparent;

            Label LblPrecio = new Label();
            LblPrecio.Content = "Precio:";
            LblPrecio.Foreground = Brushes.Gold;
            LblPrecio.FontSize = 20;
            LblPrecio.HorizontalAlignment = HorizontalAlignment.Center;
            LblPrecio.VerticalAlignment = VerticalAlignment.Center;

            precio = new TextBox();
            precio.Background = Brushes.Transparent;
            precio.TextAlignment = TextAlignment.Center;
            precio.Foreground = Brushes.Gold;
            precio.FontSize = 18;
            precio.MaxLength = 20;

            st2.Children.Add(LblPrecio);
            st2.Children.Add(precio);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st3);
            Grid.SetRow(st3, 3);
            Grid.SetColumn(st3, 0);

            // Stock

            StackPanel st4 = new StackPanel();
            st3.Orientation = Orientation.Vertical;
            st3.Background = Brushes.Transparent;

            Label LblStock = new Label();
            LblStock.Content = "stock:";
            LblStock.Foreground = Brushes.Gold;
            LblStock.FontSize = 20;
            LblStock.HorizontalAlignment = HorizontalAlignment.Center;
            LblStock.VerticalAlignment = VerticalAlignment.Center;

            stock = new TextBox();
            stock.Background = Brushes.Transparent;
            stock.TextAlignment = TextAlignment.Center;
            stock.Foreground = Brushes.Gold;
            stock.FontSize = 18;
            stock.MaxLength = 20;

            st3.Children.Add(LblStock);
            st3.Children.Add(stock);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st4);
            Grid.SetRow(st4, 4);
            Grid.SetColumn(st4, 0);


            DockPanel st6 = new DockPanel();
            st6.VerticalAlignment = VerticalAlignment.Bottom;
            st6.Background = Brushes.Transparent;

            StackPanel st7 = new StackPanel();
            st7.Orientation = Orientation.Vertical;
            st7.HorizontalAlignment = HorizontalAlignment.Right;
            st7.Background = Brushes.Transparent;

            StackPanel st8 = new StackPanel();
            st8.Orientation = Orientation.Vertical;
            st8.HorizontalAlignment = HorizontalAlignment.Left;
            st8.Background = Brushes.Transparent;

            Image b1 = new Image();
            b1.MouseLeftButtonDown += ButtonEditarProducto_Click;
            b1.MouseEnter += Boton_Hover;
            b1.MouseLeave += Boton_Leave;

            Image b2 = new Image();
            b2.MouseLeftButtonDown += AutorellenarProducto_Click;
            b2.MouseEnter += Boton_Hover;
            b2.MouseLeave += Boton_Leave;

            Label l1 = new Label();
            l1.Content = "/CONFIRMAR CAMBIOS/";
            l1.Foreground = Brushes.Gold;
            l1.FontSize = 20;
            l1.HorizontalAlignment = HorizontalAlignment.Right;
            l1.VerticalAlignment = VerticalAlignment.Bottom;

            Label l2 = new Label();
            l2.Content = "/RELLENAR CON NOMBRE/";
            l2.Foreground = Brushes.Gold;
            l2.FontSize = 20;
            l2.HorizontalAlignment = HorizontalAlignment.Left;
            l2.VerticalAlignment = VerticalAlignment.Bottom;


            BitmapImage b3 = new BitmapImage();
            b3.BeginInit();
            b3.UriSource = new Uri("./../../images/boton.png", UriKind.Relative);
            b3.EndInit();
            b1.Source = b3;
            b2.Source = b3;
            b1.Width = 170;
            b1.Height = 80;
            b2.Width = 170;
            b2.Height = 80;

            st7.Children.Add(b1);
            st7.Children.Add(l1);
            st8.Children.Add(b2);
            st8.Children.Add(l2);
            st6.Children.Add(st7);
            st6.Children.Add(st8);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st6);
            Grid.SetRow(st6, 5);
            Grid.SetColumn(st6, 5);
        }

        private void ModificarEmpleado_Click(object sender, RoutedEventArgs e)
        {
            GridDatos.Children.Clear();
            GridDatos.RowDefinitions.Clear();

            // Modificar empleado

            Label ModificarEmp = new Label();
            ModificarEmp.Content = "+Modificar Empleado+";
            ModificarEmp.Foreground = Brushes.Gold;
            ModificarEmp.FontSize = 20;
            ModificarEmp.HorizontalAlignment = HorizontalAlignment.Center;
            ModificarEmp.VerticalAlignment = VerticalAlignment.Center;

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(ModificarEmp);
            Grid.SetRow(ModificarEmp, 0);
            Grid.SetColumn(ModificarEmp, 0);

            // DNI

            StackPanel st1 = new StackPanel();
            st1.Orientation = Orientation.Vertical;
            st1.Background = Brushes.Transparent;

            Label LblDni = new Label();
            LblDni.Content = "DNI:";
            LblDni.Foreground = Brushes.Gold;
            LblDni.FontSize = 20;
            LblDni.HorizontalAlignment = HorizontalAlignment.Center;
            LblDni.VerticalAlignment = VerticalAlignment.Center;

            dni = new TextBox();
            dni.Background = Brushes.Transparent;
            dni.TextAlignment = TextAlignment.Center;
            dni.Foreground = Brushes.Gold;
            dni.FontSize = 18;
            dni.MaxLength = 20;

            st1.Children.Add(LblDni);
            st1.Children.Add(dni);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st1);
            Grid.SetRow(st1, 1);
            Grid.SetColumn(st1, 0);

            // Contraseña

            StackPanel st2 = new StackPanel();
            st2.Orientation = Orientation.Vertical;
            st2.Background = Brushes.Transparent;

            Label LblContrasena = new Label();
            LblContrasena.Content = "Contraseña:";
            LblContrasena.Foreground = Brushes.Gold;
            LblContrasena.FontSize = 20;
            LblContrasena.HorizontalAlignment = HorizontalAlignment.Center;
            LblContrasena.VerticalAlignment = VerticalAlignment.Center;

            contrasena = new TextBox();
            contrasena.Background = Brushes.Transparent;
            contrasena.TextAlignment = TextAlignment.Center;
            contrasena.Foreground = Brushes.Gold;
            contrasena.FontSize = 18;
            contrasena.MaxLength = 20;

            st2.Children.Add(LblContrasena);
            st2.Children.Add(contrasena);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st2);
            Grid.SetRow(st2, 2);
            Grid.SetColumn(st2, 0);

            // Puesto

            StackPanel st3 = new StackPanel();
            st3.Orientation = Orientation.Vertical;
            st3.Background = Brushes.Transparent;

            Label LblPuesto = new Label();
            LblPuesto.Content = "Puesto:";
            LblPuesto.Foreground = Brushes.Gold;
            LblPuesto.FontSize = 20;
            LblPuesto.HorizontalAlignment = HorizontalAlignment.Center;
            LblPuesto.VerticalAlignment = VerticalAlignment.Center;

            puesto = new TextBox();
            puesto.Background = Brushes.Transparent;
            puesto.TextAlignment = TextAlignment.Center;
            puesto.Foreground = Brushes.Gold;
            puesto.FontSize = 18;
            puesto.MaxLength = 20;

            st3.Children.Add(LblPuesto);
            st3.Children.Add(puesto);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st3);
            Grid.SetRow(st3, 3);
            Grid.SetColumn(st3, 0);

            // Teléfono

            StackPanel st4 = new StackPanel();
            st4.Orientation = Orientation.Vertical;
            st4.Background = Brushes.Transparent;

            Label LblTelefono = new Label();
            LblTelefono.Content = "Telefono:";
            LblTelefono.Foreground = Brushes.Gold;
            LblTelefono.FontSize = 20;
            LblTelefono.HorizontalAlignment = HorizontalAlignment.Center;
            LblTelefono.VerticalAlignment = VerticalAlignment.Center;

            telefono = new TextBox();
            telefono.Background = Brushes.Transparent;
            telefono.TextAlignment = TextAlignment.Center;
            telefono.Foreground = Brushes.Gold;
            telefono.FontSize = 18;
            telefono.MaxLength = 20;

            st4.Children.Add(LblTelefono);
            st4.Children.Add(telefono);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st4);
            Grid.SetRow(st4, 4);
            Grid.SetColumn(st4, 0);

            // E-mail

            StackPanel st5 = new StackPanel();
            st5.Orientation = Orientation.Vertical;
            st5.Background = Brushes.Transparent;

            Label LblEmail = new Label();
            LblEmail.Content = "Email:";
            LblEmail.Foreground = Brushes.Gold;
            LblEmail.FontSize = 20;             // Genio quien lea esto xD
            LblEmail.HorizontalAlignment = HorizontalAlignment.Center;
            LblEmail.VerticalAlignment = VerticalAlignment.Center;

            email = new TextBox();
            email.Background = Brushes.Transparent;
            email.TextAlignment = TextAlignment.Center;
            email.Foreground = Brushes.Gold;
            email.FontSize = 18;
            email.MaxLength = 20;

            st5.Children.Add(LblEmail);
            st5.Children.Add(email);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st5);
            Grid.SetRow(st5, 5);
            Grid.SetColumn(st5, 0);

            DockPanel st6 = new DockPanel();
            st6.VerticalAlignment = VerticalAlignment.Bottom;
            st6.Background = Brushes.Transparent;

            StackPanel st7 = new StackPanel();
            st7.Orientation = Orientation.Vertical;
            st7.HorizontalAlignment = HorizontalAlignment.Right;
            st7.Background = Brushes.Transparent;

            StackPanel st8 = new StackPanel();
            st8.Orientation = Orientation.Vertical;
            st8.HorizontalAlignment = HorizontalAlignment.Left;
            st8.Background = Brushes.Transparent;

            Image b1 = new Image();
            b1.MouseLeftButtonDown += ButtonEditar_Click;
            b1.MouseEnter += Boton_Hover;
            b1.MouseLeave += Boton_Leave;

            Image b2 = new Image();
            b2.MouseLeftButtonDown += Autorellenar_Click;
            b2.MouseEnter += Boton_Hover;
            b2.MouseLeave += Boton_Leave;

            Label l1 = new Label();
            l1.Content = "/CONFIRMAR CAMBIOS/";
            l1.Foreground = Brushes.Gold;
            l1.FontSize = 20;
            l1.HorizontalAlignment = HorizontalAlignment.Right;
            l1.VerticalAlignment = VerticalAlignment.Bottom;

            Label l2 = new Label();
            l2.Content = "/RELLENAR CON DNI/";
            l2.Foreground = Brushes.Gold;
            l2.FontSize = 20;
            l2.HorizontalAlignment = HorizontalAlignment.Left;
            l2.VerticalAlignment = VerticalAlignment.Bottom;


            BitmapImage b3 = new BitmapImage();
            b3.BeginInit();
            b3.UriSource = new Uri("./../../images/boton.png", UriKind.Relative);
            b3.EndInit();
            b1.Source = b3;
            b2.Source = b3;
            b1.Width = 170;
            b1.Height = 80;
            b2.Width = 170;
            b2.Height = 80;

            st7.Children.Add(b1);
            st7.Children.Add(l1);
            st8.Children.Add(b2);
            st8.Children.Add(l2);
            st6.Children.Add(st7);
            st6.Children.Add(st8);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st6);
            Grid.SetRow(st6, 6);
            Grid.SetColumn(st6, 0);
        }

        private void BorrarProducto_Click(object sender, RoutedEventArgs e)
        {
            GridDatos.Children.Clear();
            GridDatos.RowDefinitions.Clear();

            // Eliminar producto

            Label CrearEmp = new Label();
            CrearEmp.Content = "+Borrar Producto+";
            CrearEmp.Foreground = Brushes.Gold;
            CrearEmp.FontSize = 20;
            CrearEmp.HorizontalAlignment = HorizontalAlignment.Center;
            CrearEmp.VerticalAlignment = VerticalAlignment.Center;

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(CrearEmp);
            Grid.SetRow(CrearEmp, 0);
            Grid.SetColumn(CrearEmp, 0);

            // Nombre producto

            StackPanel st1 = new StackPanel();
            st1.Orientation = Orientation.Vertical;
            st1.Background = Brushes.Transparent;

            Label LblNombre = new Label();
            LblNombre.Content = "Nombre:";
            LblNombre.Foreground = Brushes.Gold;
            LblNombre.FontSize = 20;
            LblNombre.HorizontalAlignment = HorizontalAlignment.Center;
            LblNombre.VerticalAlignment = VerticalAlignment.Center;

            nombre = new TextBox();
            nombre.Background = Brushes.Transparent;
            nombre.TextAlignment = TextAlignment.Center;
            nombre.Foreground = Brushes.Gold;
            nombre.FontSize = 18;
            nombre.MaxLength = 20;

            st1.Children.Add(LblNombre);
            st1.Children.Add(nombre);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st1);
            Grid.SetRow(st1, 1);
            Grid.SetColumn(st1, 0);


            StackPanel st2 = new StackPanel();
            st2.Orientation = Orientation.Vertical;
            st2.Background = Brushes.Transparent;

            Image b1 = new Image();
            b1.MouseLeftButtonDown += ButtonBorrarProd_Click;
            b1.MouseEnter += Boton_Hover;
            b1.MouseLeave += Boton_Leave;

            Label l1 = new Label();
            l1.Content = "/Eliminar/";
            l1.Foreground = Brushes.Gold;
            l1.FontSize = 20;
            l1.HorizontalAlignment = HorizontalAlignment.Center;
            l1.VerticalAlignment = VerticalAlignment.Center;


            BitmapImage b3 = new BitmapImage();
            b3.BeginInit();
            b3.UriSource = new Uri("./../../images/boton.png", UriKind.Relative);
            b3.EndInit();
            b1.Source = b3;
            b1.Width = 170;
            b1.Height = 80;

            st2.Children.Add(b1);
            st2.Children.Add(l1);

            GridDatos.RowDefinitions.Add(new RowDefinition());
            GridDatos.Children.Add(st2);
            Grid.SetRow(st2, 8);
            Grid.SetColumn(st2, 0);
        }

        public void Boton_Hover(object sender, MouseEventArgs e)
        {
            Image l1 = sender as Image;

            BitmapImage b3 = new BitmapImage();
            b3.BeginInit();
            b3.UriSource = new Uri("./images/boton_focus.png", UriKind.Relative);
            b3.EndInit();

            l1.Source = b3;
        }

        public void Boton_Leave(object sender, MouseEventArgs e)
        {
            Image l1 = sender as Image;

            BitmapImage b3 = new BitmapImage();
            b3.BeginInit();
            b3.UriSource = new Uri("./../../images/boton.png", UriKind.Relative);
            b3.EndInit();

            l1.Source = b3;
        }

        private void CrearEmpleado_Click(object sender, MouseButtonEventArgs e)
        {
            if (nombre.Text == null || nombre.Text.Equals("") || nombre.Text.Equals(" "))
            {
                MessageBox.Show("El nombre no puede estar vacío.");
                return;

            }
            else if (apellidos.Text == null || apellidos.Text.Equals("") || apellidos.Text.Equals(" "))
            {
                MessageBox.Show("El apellido no puede estar vacío.");
                return;

            }
            else if (dni.Text == null || dni.Text.Equals("") || dni.Text.Equals(" "))
            {
                MessageBox.Show("El DNI no puede estar vacío.");
                return;

            }
            else if (contrasena.Text == null || contrasena.Text.Equals("") || contrasena.Text.Equals(" "))
            {
                MessageBox.Show("La contraseña no puede estar vacía.");
                return;

            }
            else if (email.Text == null || email.Text.Equals("") || email.Text.Equals(" ") || !email.Text.Contains("@"))
            {
                MessageBox.Show("El e-mail no puede estar vacío o no es válido (comprueba que tenga @).");
                return;

            }
            else if (telefono.Text == null || telefono.Text.Equals("") || telefono.Text.Equals(" "))
            {
                MessageBox.Show("El teléfono no puede estar vacío.");
                return;

            }
            else if (puesto.Text == null || puesto.Text.Equals("") || puesto.Text.Equals(" "))
            {
                MessageBox.Show("El puesto no puede estar vacío.");
                return;

            }

            string id = "/empleado/" + dni.Text;

            var request = new RestRequest(id, Method.Get);
            var response = client.GetAsync(request);

            try
            {
                if (!response.Result.Content.Equals("null"))
                {
                    MessageBox.Show("Ya existe un empleado con este DNI");
                    return;
                }
            }
            catch (Exception ex) { }

            CrearEmpleado(nombre.Text, apellidos.Text, contrasena.Text, dni.Text, puesto.Text, telefono.Text, email.Text);
        }

        private void ButtonBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dni.Text.Equals(""))
            {
                MessageBox.Show("El DNI a eliminar no puede estar vacio.");
                return;
            }
            else if (dni.Text == null)
            {
                MessageBox.Show("Wtf. No se como has hecho eso, pero negated.");
                return; // Si lees esto, eres un mastodonte máquina champion leyenda
            }

            BorrarEmpleado(dni.Text);
        }

        private void ButtonEditar_Click(object sender, RoutedEventArgs e)
        {
            if (dni.Text.Equals(""))
            {
                MessageBox.Show("El DNI del empleado a editar no puede estar vacio.");
                return;
            }
            else if (dni.Text == null)
            {
                MessageBox.Show("Wtf. No se como has hecho eso, pero negated.");
                return;
            }
            else if (contrasena.Text == null || contrasena.Text.Equals("") || contrasena.Text.Equals(" "))
            {
                MessageBox.Show("La contraseña no puede estar vacía.");
                return;

            }
            else if (puesto.Text == null || puesto.Text.Equals("") || puesto.Text.Equals(" "))
            {
                MessageBox.Show("El puesto no puede estar vacío.");
                return;

            }
            else if (telefono.Text == null || telefono.Text.Equals("") || telefono.Text.Equals(" "))
            {
                MessageBox.Show("El teléfono no puede estar vacío.");
                return;

            }
            else if (email.Text == null || email.Text.Equals("") || email.Text.Equals(" ") || !email.Text.Contains("@"))
            {
                MessageBox.Show("El e-mail no puede estar vacío o no es válido (comprueba que tenga @).");
                return;

            }

            EditarEmpleado(dni.Text, contrasena.Text, puesto.Text, telefono.Text, email.Text);
        }

        private void Autorellenar_Click(object sender, RoutedEventArgs e)
        {
            if (dni.Text.Equals(""))
            {
                MessageBox.Show("El DNI de autorellenar no puede estar vacio.");
                return;
            }
            else if (dni.Text == null)
            {
                MessageBox.Show("Wtf. No se como has hecho eso, pero negated.");
                return;
            }

            string id = "/empleado/" + dni.Text;

            var request = new RestRequest(id, Method.Get);
            var response = client.GetAsync(request);
            Empleado emp1 = new Empleado();

            try
            {
                if (!response.Result.Content.Equals("null"))
                    emp1 = JsonConvert.DeserializeObject<Empleado>(response.Result.Content);
                else
                {
                    MessageBox.Show("No hay empleados con el DNI especificado");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }

            contrasena.Text = emp1.Contraseña;
            puesto.Text = emp1.Puesto;
            telefono.Text = emp1.Telefono;
            email.Text = emp1.Email;
        }

        private void ButtonEditarProducto_Click(object sender, RoutedEventArgs e)
        {
            double doublePrecio = 0;
            int intStock = 0;

            if (nombre.Text.Equals(""))
            {
                MessageBox.Show("El nombre no puede estar vacío. Inserta el nombre del producto a modificar.");
                return;
            }
            else if (specs.Text == null || specs.Text.Equals(""))
            {
                MessageBox.Show("Las especificaciones no son válidas.");
                return;
            }
            else if (precio.Text == null || precio.Text.Equals("") || precio.Text.Contains(" ") || !Double.TryParse(precio.Text, out doublePrecio))
            {
                MessageBox.Show("El precio no es válido.");
                return;
            }
            else if (stock.Text == null || stock.Text.Equals("") || stock.Text.Contains(" ") || !int.TryParse(stock.Text, out intStock))
            {
                MessageBox.Show("El stock no es válido.");
                return;

            }

            EditarProducto(specs.Text, doublePrecio, intStock);
        }

        private void AutorellenarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (nombre.Text.Equals(""))
            {
                MessageBox.Show("El nombre de autorellenar no puede estar vacio.");
                return;
            }
            else if (nombre.Text == null)
            {
                MessageBox.Show("Wtf. No se como has hecho eso, pero negated.");
                return;
            }

            string id = "/producto/" + nombre.Text;

            var request = new RestRequest(id, Method.Get);
            var response = client.GetAsync(request);
            Producto pr1 = new Producto();

            try
            {
                if (!response.Result.Content.Equals("null"))
                    pr1 = JsonConvert.DeserializeObject<Producto>(response.Result.Content);
                else
                {
                    MessageBox.Show("No hay productos con ese nombre. Asegúrate de poner bien las minúsculas y mayúsculas");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }

            specs.Text = pr1.Especificaciones;
            precio.Text = pr1.Precio.ToString();
            stock.Text = pr1.Stock.ToString();
        }

        private void ButtonBorrarProd_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest("/producto/" + nombre.Text, Method.Get);
            var response = client.GetAsync(request);

            try
            {
                if (response.Result.Content.Equals("null"))
                {
                    MessageBox.Show("No hay ningún producto con ese nombre");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido problemas conectando con la base de datos, compruebe su conexión.");
                return;
            }

            BorrarProducto(nombre.Text);
        }


        private void NuevoProducto_Click(object sender, RoutedEventArgs e)
        {
            double doublePrecio = 0;
            int intStock = 0;
            bool sameProductFound = false;

            if (nombre.Text == null || nombre.Text.Equals("") || nombre.Text.Equals(" "))
            {
                MessageBox.Show("El nombre no puede estar vacío.");
                return;

            }
            else if (specs.Text == null || specs.Text.Equals("") || specs.Text.Equals(" "))
            {
                MessageBox.Show("Las especificaciones no pueden estar vacías.");
                return;

            }
            else if (precio.Text == null || precio.Text.Equals("") || precio.Text.Contains(" ") || !Double.TryParse(precio.Text, out doublePrecio))
            {
                MessageBox.Show("El precio no es válido.");
                return;

            }
            else if (imagen.Text == null || imagen.Text.Equals("") || imagen.Text.Contains(" "))
            {
                MessageBox.Show("La imagen no puede estar vacía ni contener espacios.");
                return;

            }
            else if (stock.Text == null || stock.Text.Equals("") || stock.Text.Contains(" ") || !int.TryParse(stock.Text, out intStock))
            {
                MessageBox.Show("El stock no es válido.");
                return;

            }
            else if (tipoprod.Text == null || tipoprod.Text.Equals("") || tipoprod.Text.Equals(" "))
            {
                MessageBox.Show("El tipo de producto no puede estar vacío.");
                return;
            }

            List<Producto> prList = new List<Producto>();
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

            if (prList.Count < 1)
                return;

            prList.ForEach(m =>
            {
                if (nombre.Text.ToLower().Equals(m.Nombre.ToLower()))
                {
                    MessageBox.Show("Ya existe un producto con este nombre en la base de datos.");
                    sameProductFound = true;
                    return;
                }
            });

            bool wasCategoria = es_categoria.IsChecked ?? false;

            NuevoProducto(nombre.Text, specs.Text, Double.Parse(precio.Text), imagen.Text, Int32.Parse(stock.Text), tipoprod.Text, wasCategoria);
        }

        private void TextboxOnlyInt(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
