﻿using Newtonsoft.Json;
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
            if (descuentocalc.Text.Length > 9)
            {
                return;
            }
            Button b1 = sender as Button;
            descuentocalc.Text += b1.Content;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            descuentocalc.Text = "";
        }

        private void Tarjeta_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("A casa champion");
            Close();
        }
    }
}
