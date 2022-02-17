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
    /// <summary>
    /// Lógica de interacción para ProductPreview.xaml
    /// </summary>
    public partial class ProductPreview : Window
    {
        public ProductPreview(Producto p)
        {
            InitializeComponent();

            Image i = new Image();
            i.Source = new BitmapImage(new Uri(p.Imagen, UriKind.RelativeOrAbsolute));

            GridProduct.Children.Add(i);
            Grid.SetRow(i, 0);
            Grid.SetColumn(i, 0);

            name.Text = p.Nombre;
            stock.Text = "Stock: " + p.Stock;
            precio.Text = p.Precio + "€";
            description.Text = p.Especificaciones;
        }
    }
}
