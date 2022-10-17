using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazTP
{
    public class Pago
    {
        public int id { get; set; }
        public Usuario user { get; set; }
        public string nombre { get; set; }
        public float monto { get; set; }
        public bool pagado { get; set; }
        public string metodo { get; set; }

        public Pago()
        {

        }

        public string[] toArray()
        {
            return new string[] { id.ToString(), nombre, monto.ToString(), pagado.ToString() };
        }

    }
}
