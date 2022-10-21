using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazTP
{
    public class TarjetaDeCredito
    {
        public int id { get; set; }
        public Usuario titular { get; set; }
        public int numero { get; set; }
        public int odigoV { get; set; }
        public float limite { get; set; }
        public float consumos { get; set; }

        public TarjetaDeCredito()
        {

        }

        public string[] toArray()
        {
            return new string[] { numero.ToString(), odigoV.ToString(), limite.ToString(), consumos.ToString() };
        }

    }
}
