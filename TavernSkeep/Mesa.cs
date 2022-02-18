using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernSkeep
{
    class Mesa
    {
        private string _id = "";
        private string codigo = "";
        private string zona = "";
        private int n_sillas = 0;
        private bool is_reservada = false;
        private string ticket_actual = "";

        public string Id { get => _id; set => _id = value; }
        public string Codigo { get => codigo; set => codigo = value; }
        public string Zona { get => zona; set => zona = value; }
        public int N_sillas { get => n_sillas; set => n_sillas = value; }
        public bool Is_reservada { get => is_reservada; set => is_reservada = value; }
        public string Ticket_actual { get => ticket_actual; set => ticket_actual = value; }
    }
}
