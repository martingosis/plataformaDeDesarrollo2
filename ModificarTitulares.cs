using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfazTP
{
    public partial class ModificarTitulares : Form
    {
        Banco banco;
        public TransDelegadoVolverAtrasTitulares TransfEventoVolver;

        public ModificarTitulares(Banco banco)
        {
            InitializeComponent();
            this.banco = banco;
            refreshDataTitulares();
        }

        // Boton Volver
        private void button3_Click(object sender, EventArgs e)
        {
            bool confirmacion = true;
            this.TransfEventoVolver(confirmacion);
        }

        public delegate void TransDelegadoVolverAtrasTitulares(bool confirmacion);

        // Boton Eliminar Titular
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        // Boton Agregar Titular
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            banco.MostrarCajasDeAhorro();
            for(int i = 0; i < comboBox1.Items.Count; i++)
            {
                comboBox1.Items.Add(banco.MostrarCajasDeAhorro());
            }
        }

        private void refreshDataTitulares()
        {
            comboBox1.Items.Clear();

            foreach(CajaDeAhorro c in banco.usuarioActual.obtenerCajas())
            {
                comboBox1.Items.AddRange(new object[] {c.cbu.ToString()});
            }
        }
    }
}
