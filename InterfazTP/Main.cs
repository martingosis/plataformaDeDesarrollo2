using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
        public TransDelegadoModificarTitularesCajaAhorro TransFModificarTitulares;
        public TransDelegadoCerrarSesion TransFCerrarSesion;
        private int selectedCaja;
        private int selectedPLazoFijo;

        public Main(string usuario, Banco banco)
        {
            InitializeComponent();
            this.usuario = usuario;
            this.banco = banco;
            refreshDataCbu();
            refreshDataPlazoFijo();
            OBSPLazosFijos();
            label9.Text = PlazoFijo.tasa.ToString();
        }

        public Main(object[] args)
        {
            InitializeComponent();
            banco = (Banco)args[1];
            argumentos = args;
            //label2.Text = (string)args[0];
            datos = new List<List<string>>();
            refreshDataCbu();
            refreshDataPlazoFijo();
            OBSPLazosFijos();
        }

        // Modificar Usuario
        private void button12_Click(object sender, EventArgs e)
        {
            bool modificar = true;
            this.TransFModificarUsuario(modificar);
        }

        public delegate void TransDelegadoModificarUsuario(bool modificar);
        public delegate void TransDelegadoModificarTitularesCajaAhorro(bool modificar);
        public delegate void TransDelegadoCerrarSesion(bool modificar);

        // CAJA DE AHORRO ////////////////////////////////////////////////////////////

        private void refreshDataCajaDeAhorro()
        {
            dataGridView1.Rows.Clear();

            foreach(CajaDeAhorro c in banco.usuarioActual.obtenerCajas())
            {
                dataGridView1.Rows.Add(c.toArray());
            }
        }

        private void refreshDataCbu()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            foreach (CajaDeAhorro c in banco.usuarioActual.obtenerCajas())
            {
                comboBox1.Items.Add(c.cbu.ToString());
                comboBox3.Items.Add(c.cbu.ToString());
            }

            foreach (CajaDeAhorro c in banco.MostrarCajasDeAhorro())
            {
                    comboBox2.Items.Add(c.cbu.ToString());
            }
        }

        private void refreshDataCajaMovimientos()
        {
            dataGridView5.Rows.Clear();

            foreach (Movimiento m in   banco.MostrarMovimientos(this.selectedCaja))
            {
                dataGridView5.Rows.Add(m.toArray());
            }
        }

        // Seleccionador de caja
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedCaja = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value);
            if (selectedCaja != null && selectedCaja != 0)
            {
                refreshDataCajaMovimientos();
            }
        }

        // Agregar Caja Ahorro
        private void button1_Click(object sender, EventArgs e)
        {
            banco.AltaCajaAhorro(banco.usuarioActual);
            MessageBox.Show("Se agrego una nueva Caja de Ahorro.");
            refreshDataCajaDeAhorro();
            refreshDataCbu();
        }


        //Eliminar Caja de Ahorro
        private void button3_Click(object sender, EventArgs e)
        {
            if (banco.BajaCajaAhorro(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value)))
            {
                selectedCaja = banco.usuarioActual.obtenerCajas().First().id;
                refreshDataCbu();
                refreshDataCajaDeAhorro();
            }
            else
            {
                MessageBox.Show("No se pudo eliminar la cuenta");
            }
            

        }

        //Retirar
        private void button15_Click(object sender, EventArgs e)
        {
            banco.Retirar(selectedCaja, Convert.ToInt32(textBox1.Text));
            refreshDataCajaMovimientos();
            refreshDataCajaDeAhorro();
        }
        //Depositar
        private void button14_Click(object sender, EventArgs e)
        {
            banco.Depositar(selectedCaja, Convert.ToInt32(textBox1.Text));
            refreshDataCajaMovimientos();
            refreshDataCajaDeAhorro();
        }
        //Transferir
        private void button13_Click(object sender, EventArgs e)
        {
            banco.Transferir(Convert.ToInt32(comboBox1.Text), Convert.ToInt32(comboBox2.Text), Convert.ToInt32(textBox2.Text));
            refreshDataCajaMovimientos();
            refreshDataCajaDeAhorro();
        }

        // PLAZOS FIJOS ////////////////////////////////////////////////////////////
        private void refreshDataPlazoFijo()
        {
            dataGridView2.Rows.Clear();

            foreach (PlazoFijo p in banco.usuarioActual.pf)
            {
                dataGridView2.Rows.Add(p.toArray());
            }
        }

        // Cobrar plazos fijos si se cumplio la fecha
        private void OBSPLazosFijos()
        {
            foreach(PlazoFijo p in banco.usuarioActual.pf)
            {
                if (DateTime.Now >= p.fechaFin && p.pagado !=true)
                {
                    banco.cobrarPlazoFijo(p.id);
                }
            }
        }

        //Agregar Plazo Fijo
        private void button4_Click(object sender, EventArgs e)
        {
            if (banco.AltaPlazoFijo(new PlazoFijo(banco.usuarioActual, float.Parse(textBox3.Text)), Convert.ToInt32(comboBox3.Text)))
            {
                MessageBox.Show("Se creo tu plazo fijo");
                refreshDataPlazoFijo();
            }
            else
            {
                MessageBox.Show("Hubo un problema al crear el plazo fijo");
            }
        }

        //Eliminar Plazo Fijo
        private void button6_Click(object sender, EventArgs e)
        {
            
        }


        // PAGOS ////////////////////////////////////////////////////////////
        private void refreshDataPagos()
        {
            dataGridView3.Rows.Clear();

            foreach (Pago p in banco.usuarioActual.pagos)
            {
                dataGridView3.Rows.Add(p.toArray());
            }
        }

        // TARJETAS DE CREDITO ////////////////////////////////////////////////////////////
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }


        // Boton para Modificar Titulares de Caja de Ahorro
        private void button2_Click(object sender, EventArgs e)
        {
            bool resultado = true;
            this.TransFModificarTitulares(resultado);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {


        }

        // Boton cerrar sesion
        private void button16_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Queres cerrar sesion?", "Advertencia", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {
                bool confirmacion = true;
                this.TransFCerrarSesion(confirmacion);
                banco.CerrarSesion();
            }
        }
    }
}
