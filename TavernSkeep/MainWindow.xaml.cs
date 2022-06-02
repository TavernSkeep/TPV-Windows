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
    public partial class MainWindow : Window
    {
        int intentos = 0;
        RestClient client = new RestClient("https://tavernskeep-api.herokuapp.com");
        Empleado emp1;
        //static Cursor sword = new Cursor(Application.GetResourceStream(new Uri("sword.cur")).Stream);
        public MainWindow()
        {
            InitializeComponent();
            this.Cursor = Cursors.None;
            Cursor cursor = new Cursor(Application.GetResourceStream(new Uri("cursors/sword.cur", UriKind.Relative)).Stream);
            this.Cursor = cursor;

            MiniminizeButton.Click += (s, e) => WindowState = WindowState.Minimized;
            MaximizeButton.Click += (s, e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            CloseButton.Click += (s, e) => Close();


            //Mouse.OverrideCursor = sword;
            Console.WriteLine("Always skeep = true");
        }

        // Evento del botón para iniciar sesión
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            string id = "/empleado/" + dni.Text;

            var request = new RestRequest(id, Method.Get);
            var response = client.GetAsync(request);
            emp1 = new Empleado();
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
                intentos++;

            }
            else if (contraseña1.Password.Equals(null) || contraseña1.Password.Equals("") || contraseña1.Password.Equals(" "))
            {
                MessageBox.Show("Se requiere introducir la contraseña del empleado.");
                intentos++;

            }
            else if (response.Result.Content.Equals("null"))
            {
                MessageBox.Show("El DNI del empleado introducido es incorrecto o no existe.");
                intentos++;

            }
            else if (!emp1.Contraseña.Equals(contraseña1.Password))
            {
                MessageBox.Show("Contraseña incorrecta.");
                intentos++;
            }
            else if (emp1.Dni.Equals(dni.Text) && emp1.Contraseña.Equals(contraseña1.Password))
            {
                this.Hide();
                SkeepHub a = new SkeepHub(emp1);
                a.WindowState = this.WindowState;
                a.Show();
                this.Close();
            }

            if (intentos == 3)
            {
                MessageBox.Show("Límite de intentos para iniciar sesión alcanzado. Se cerrará la aplicación.");
                this.Close();
            }
            /*
            MessageBox.Show(myDetails.dni);
            MessageBox.Show(response.IsCompleted.ToString());
            MessageBox.Show(response.Result.Content);
            MessageBox.Show(json.ToString());

            // Estilos textbox (old)

            <TextBox.Style>
                        <Style TargetType="TextBox" xmlns:sys="clr-namespace:System;assembly=mscorlib">
                            <Style.Resources>
                                <VisualBrush x:Key="CueBannerBrush" AlignmentX="Left" AlignmentY="Center" Stretch="None">
                                    <VisualBrush.Visual>
                                        <Label Content="DNI" Foreground="LightGray" FontFamily="/TavernSkeep;component/font/#Arcade N" />
                                    </VisualBrush.Visual>
                                </VisualBrush>
                            </Style.Resources>
                            <Style.Triggers>
                                <Trigger Property="Text" Value="{x:Static sys:String.Empty}">
                                    <Setter Property="Background" Value="{StaticResource CueBannerBrush}" />
                                </Trigger>
                                <Trigger Property="Text" Value="{x:Null}">
                                    <Setter Property="Background" Value="{StaticResource CueBannerBrush}" />
                                </Trigger>
                                <Trigger Property="IsKeyboardFocused" Value="True">
                                    <Setter Property="Background" Value="White" />
                                </Trigger>
                            </Style.Triggers>
                        </Style>
                    </TextBox.Style>

            <PasswordBox.Style>
                        <Style TargetType="PasswordBox" xmlns:sys="clr-namespace:System;assembly=mscorlib">
                            <Style.Resources>
                                <VisualBrush x:Key="CueBannerBrush" AlignmentX="Left" AlignmentY="Center" Stretch="None">
                                    <VisualBrush.Visual>
                                        <Label Content="Password" Foreground="LightGray" FontFamily="/TavernSkeep;component/font/#Arcade N"  />
                                    </VisualBrush.Visual>
                                </VisualBrush>
                            </Style.Resources>
                            <Style.Triggers>
                                <Trigger Property="Password" Value="{x:Static sys:String.Empty}">
                                    <Setter Property="Background" Value="{StaticResource CueBannerBrush}" />
                                </Trigger>
                                <Trigger Property="Password" Value="{x:Null}">
                                    <Setter Property="Background" Value="{StaticResource CueBannerBrush}" />
                                </Trigger>
                                <Trigger Property="IsKeyboardFocused" Value="True">
                                    <Setter Property="Background" Value="White" />
                                </Trigger>
                            </Style.Triggers>
                        </Style>
                    </PasswordBox.Style>
            */
        }
    }
}
