﻿using RestSharp;
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
        public List<LineaTicket> ticketmesa = new List<LineaTicket>();
        public List<LineaTicket> TicketVuelta
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

                Image b1 = new Image();
                Label l1 = new Label();
                l1.Content = m.Codigo;
                l1.Tag = m;

                BitmapImage b3 = new BitmapImage();
                b3.BeginInit();
                b3.UriSource = new Uri("./images/boton.png", UriKind.Relative);
                b3.EndInit();

                b1.Source = b3;
                               
                l1.MouseLeftButtonDown += mesas_Click;
                l1.VerticalAlignment = VerticalAlignment.Center;
                l1.HorizontalAlignment = HorizontalAlignment.Center;

                if (m.Ticket_actual.Equals(""))
                    l1.Foreground = Brushes.Green;
                else
                    l1.Foreground = Brushes.Red;

                if (m.Is_reservada)
                    l1.Foreground = Brushes.Orange;

                GridBotones.Children.Add(b1);
                Grid.SetColumn(b1, columna);
                Grid.SetRow(b1, fila);

                GridBotones.Children.Add(l1);
                Grid.SetColumn(l1, columna);
                Grid.SetRow(l1, fila);

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
            Label buttonmesa = sender as Label;
            Mesa m = buttonmesa.Tag as Mesa;

            if (m.Ticket_actual.Equals(""))
            {
                Random rand = new Random();
                Ticket tick = new Ticket();
                tick.Id = "t" + rand.Next();
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

                ticketmesa = t1.Listaproductos;

                foreach (LineaTicket lt in ticketmesa)
                {
                    MessageBox.Show(lt.Nombre);
                }

                Close();
            }

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