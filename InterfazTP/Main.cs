using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfazTP
{
    public partial class Main : Form
    {
        public object[] argumentos;
        List<List<string>> datos;
        public string usuario;
        public Banco banco;
        public TransDelegadoModificarUsuario TransFModificarUsuario;
        private int selectedCaja;

        public Main(string usuario, Banco banco)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.banco = banco;
        }

        public Main(object[] args)
        {
            InitializeComponent();
            banco = (Banco)args[1];
            argumentos = args;
            //label2.Text = (string)args[0];
            datos = new List<List<string>>();
        }

        private void refreshDataCajaDeAhorro()
        {
            dataGridView1.Rows.Clear();

            foreach(CajaDeAhorro c in banco.usuarioActual.obtenerCajas())
            {
                dataGridView1.Rows.Add(c.toArray());
            }
        }

        private void refreshDataPlazoFijo()
        {
            dataGridView2.Rows.Clear();

            foreach (PlazoFijo p in banco.usuarioActual.pf)
            {
                dataGridView2.Rows.Add(p.toArray());
            }
        }
        private void refreshDataPagos()
        {
            dataGridView3.Rows.Clear();

            foreach (Pago p in banco.usuarioActual.pagos)
            {
                dataGridView3.Rows.Add(p.toArray());
            }
        }
        private void refreshDataTarjetasDeCredito()
        {
            dataGridView4.Rows.Clear();

            foreach (TarjetaDeCredito t in banco.usuarioActual.tarjeta)
            {
                dataGridView4.Rows.Add(t.toArray());
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        // Agregar Caja Ahorro
        private void button1_Click(object sender, EventArgs e)
        {
            banco.AltaCajaAhorro(banco.usuarioActual);
            MessageBox.Show("Se agrego una nueva Caja de Ahorro.");
            refreshDataCajaDeAhorro();
        }

        // Modificar Usuario
        private void button12_Click(object sender, EventArgs e)
        {
            bool modificar = true;
            this.TransFModificarUsuario(modificar);
        }

        public delegate void TransDelegadoModificarUsuario(bool modificar);

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            banco.BajaCajaAhorro(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value));
            refreshDataCajaDeAhorro();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
