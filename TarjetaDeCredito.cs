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
        public static int ultimoId = 0;
        public Usuario titular { get; set; }
        public int numero { get; set; }
        public static int ultimoNumero = 40000000;
        public int odigoV { get; set; }
        public float limite { get; set; }
        public float consumos { get; set; }

        Random rnd = new Random();

        public TarjetaDeCredito(Usuario titular)
        {
            id = generarId();
            this.titular = titular;
            generarNumero();
            odigoV = rnd.Next(100, 999);
            limite = 100000;
            consumos = 0;
        }

        private int generarId()
        {
            ultimoId++;
            return ultimoId;
        }

        private int generarNumero()
        {
            ultimoNumero++;
            return ultimoNumero;
        }

        public string[] toArray()
        {
            return new string[] { numero.ToString(), odigoV.ToString(), limite.ToString(), consumos.ToString() };
        }

    }
}
