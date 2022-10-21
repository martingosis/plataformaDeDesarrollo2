using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazTP
{
    public class PlazoFijo
    {
        public int id { get; set; }
        public Usuario titular { get; set; }
        public float monto { get; set; }
        public DateTime fechaIni { get; set; }
        public DateTime fechaFin { get; set; }
        public float tasa { get; set; }
        public bool pagado { get; set; }

        public PlazoFijo()
        {

        }


        public string[] toArray()
        {
            return new string[] { id.ToString(), monto.ToString(), fechaIni.ToString(), fechaFin.ToString(), tasa.ToString(), pagado.ToString() };
        }
    }
}
