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
    /// <summary>
    /// Lógica de interacción para mesas.xaml
    /// </summary>
    public partial class mesas : Window
    {
        RestClient client = new RestClient("http://localhost:8080");
        ListView ListTicket;
        List<Button> BotonesMesas = new List<Button>();
        List<Mesa> ObjetosMesa = new List<Mesa>();
        //Mesa m1, m2, m3, m4, m5, m6, m7, m8, m9;
        public mesas(ListView listticket)
        {
            InitializeComponent();
            ListTicket = listticket;
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
            foreach (Mesa m in ObjetosMesas)
            {
                Button b1 = new Button();
                b1.Content = m.Codigo;
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("./images/boton.png", UriKind.Relative));
                b1.Background = brush;
                b1.Tag = m;

                GridBotones.Children.Add(b1);
                Grid.SetColumn(b1, columna);
                Grid.SetRow(b1, fila);

                columna++;
                
                if (columna > 3)
                {
                    fila++;
                    columna = 0;

                    RowDefinition rd = new RowDefinition();
                    GridBotones.RowDefinitions.Add(rd);
                }
            }
        }
        private void mesas_Click(object sender, RoutedEventArgs e)
        {
            Button mesa = sender as Button;
            
            if (mesa.Tag != null)
            {
                MessageBox.Show("Esta mesa ya tiene un ticket");
                return;
            }

            mesa.Tag = ListTicket;
        }
    }
}

/*
        <Viewbox Grid.Row="0" Grid.Column="0">
                <Button Content="MESA 1" Width="auto" Height="auto" MinHeight="150px"  MinWidth="150px" MaxWidth="250px"  MaxHeight="200px" Foreground="White"  FontFamily="/TavernSkeep;component/font/#Arcade N"
                          x:Name="mesa1" ClickMode="Press" UseLayoutRounding="True" FontSize="30" Click="mesas_Click">
                    <Button.Style >
                        <Style TargetType="{x:Type Button}">
                            <Setter Property="Template">
                                <Setter.Value>
                                    <ControlTemplate TargetType="Button">
                                        <Grid>
                                            <Image 
                                            x:Name="buttonImage" Stretch="Fill"
                                            Source="/TavernSkeep;component/images/boton.png"/>
                                            <ContentPresenter
                                            Margin="{TemplateBinding Padding}"
                                            HorizontalAlignment="Center"
                                            VerticalAlignment="Center" />
                                        </Grid>
                                        <ControlTemplate.Triggers>
                                            <Trigger Property="IsMouseOver" Value="true">
                                                <Setter TargetName="buttonImage" Property="Source" Value="/TavernSkeep;component/images/boton_focus.png" />
                                            </Trigger>
                                        </ControlTemplate.Triggers>
                                    </ControlTemplate>
                                </Setter.Value>
                            </Setter>
                        </Style>
                    </Button.Style>
                </Button>
            </Viewbox>
*/