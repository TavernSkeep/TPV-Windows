using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernSkeep
{
    public class Ticket
    {
        private string _id = "";
        private string codigo = "";
        private string mesa = "";
        private List<LineaTicket> listaproductos = new List<LineaTicket>();
        private string fecha = DateTime.Now.ToString();

        public string Id { get => _id; set => _id = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Mesa { get => mesa; set => mesa = value; }
        public List<LineaTicket> Listaproductos { get => listaproductos; set => listaproductos = value; }
        public String Fecha { get => fecha; set => fecha = value; }
    }
}
