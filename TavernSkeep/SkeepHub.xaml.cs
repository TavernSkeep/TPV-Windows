﻿using System;
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
        public SkeepHub()
        {
            InitializeComponent();
            //media.Source = new Uri();
            Loading();
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
    }
}
