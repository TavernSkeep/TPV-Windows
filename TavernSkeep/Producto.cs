using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernSkeep
{
    class Producto
    {
        private String _id = "";
        private String nombre = "";
        private String especificaciones = "";
        private Double precio = 0;
        private String imagen = "";
        private int stock = 0;
        private String tipo_producto = "";
        private Boolean es_categoria = false;

        public string Codigo { get => _id; set => _id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Especificaciones { get => especificaciones; set => especificaciones = value; }
        public double Precio { get => precio; set => precio = value; }
        public string Imagen { get => imagen; set => imagen = value; }
        public int Cantidad_stock { get => stock; set => stock = value; }
        public string Tipo_producto { get => tipo_producto; set => tipo_producto = value; }
        public bool Es_categoria { get => es_categoria; set => es_categoria = value; }
    }
}
