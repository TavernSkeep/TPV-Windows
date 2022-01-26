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
        public SkeepHub()
        {
            InitializeComponent();
            //media.Source = new Uri();
            Loading();
        }

        DispatcherTimer timer = new DispatcherTimer();

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            media.Position = new TimeSpan(0, 0, 1);
            media.Play();
        }

        private void timer_tick(Object sender, EventArgs e)
        {
            timer.Stop();
            media.Visibility = Visibility.Hidden;
            media_dots.Visibility = Visibility.Hidden;
            content.Visibility = Visibility.Visible;
        }

        void Loading()
        {
            timer.Tick += timer_tick;
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();
        }

        private void media_dots_MediaEnded(object sender, RoutedEventArgs e)
        {
            media_dots.Position = new TimeSpan(0, 0, 1);
            media_dots.Play();
        }
    }
}
