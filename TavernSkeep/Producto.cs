using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernSkeep
{
    public class Producto
    {
        private String _id = "";
        private String nombre = "";
        private String especificaciones = "";
        private Double precio = 0;
        private String imagen = "";
        private int stock = 0;
        private String tipo_producto = "";
        private Boolean es_categoria = false;

        public Producto(string nombre, string especificaciones, Double precio, string imagen, int stock, string tipo_producto, Boolean es_categoria)
        {
            Random rand = new Random();
            Id = rand.Next().ToString();
            Nombre = nombre;
            Especificaciones = especificaciones;
            Precio = precio;
            Imagen = imagen;
            Stock = stock;
            Tipo_producto = tipo_producto;
            Es_categoria = es_categoria;
        }

        public Producto() { }
        public string Id { get => _id; set => _id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Especificaciones { get => especificaciones; set => especificaciones = value; }
        public double Precio { get => precio; set => precio = value; }
        public string Imagen { get => imagen; set => imagen = value; }
        public int Stock { get => stock; set => stock = value; }
        public string Tipo_producto { get => tipo_producto; set => tipo_producto = value; }
        public bool Es_categoria { get => es_categoria; set => es_categoria = value; }
    }
}
