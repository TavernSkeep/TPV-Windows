using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernSkeep
{
    public class Empleado
    {
        private String _id = "";
        private String contraseña = "";
        private String nombre = "";
        private String apellidos = "";
        private String dni = "";
        private String puesto = "";
        private String telefono = "";
        private String email = "";

        public string Id { get => _id; set => _id = value; }
        public string Contraseña { get => contraseña; set => contraseña = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellidos { get => apellidos; set => apellidos = value; }
        public string Dni { get => dni; set => dni = value; }
        public string Puesto { get => puesto; set => puesto = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Email { get => email; set => email = value; }
    }
}
