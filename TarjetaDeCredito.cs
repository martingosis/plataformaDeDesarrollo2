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
        public DateOnly codigoV { get; set; }
        public float limite { get; set; }
        public float consumos { get; set; }
        public static int ultimoId = 0;

        public TarjetaDeCredito()
        {
            id = generarId();
        }

        private int generarId()
        {
            ultimoId++;
            return ultimoId;
        }

        public string[] toArray()
        {
            return new string[] { numero.ToString(), codigoV.ToString(), limite.ToString(), consumos.ToString() };
        }

    }
}
