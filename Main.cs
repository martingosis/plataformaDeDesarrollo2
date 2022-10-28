﻿using System;
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
        private int selectedCaja;
        private int selectedPLazoFijo;
        private int selectedPago;

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
            label9.Text = PlazoFijo.tasa.ToString();
        }

        // Modificar Usuario
        private void button12_Click(object sender, EventArgs e)
        {
            bool modificar = true;
            this.TransFModificarUsuario(modificar);
        }

        public delegate void TransDelegadoModificarUsuario(bool modificar);

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
            comboBox4.Items.Clear();

            foreach (CajaDeAhorro c in banco.usuarioActual.obtenerCajas())
            {
                comboBox1.Items.Add(c.cbu.ToString());
                comboBox3.Items.Add(c.cbu.ToString());
                comboBox4.Items.Add(c.cbu.ToString());
            }

            foreach (CajaDeAhorro c in banco.MostrarCajasDeAhorro())
            {
                    comboBox2.Items.Add(c.cbu.ToString());
            }

            foreach (TarjetaDeCredito t in banco.usuarioActual.obtenerTarjetas())
            {
                comboBox4.Items.Add(t.numero.ToString());
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
                MessageBox.Show("Se elimino correctamente");
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
            if(banco.Retirar(selectedCaja, Convert.ToInt32(textBox1.Text)))
            {
                MessageBox.Show("Se retiro correctamente: " + textBox1.Text);
                refreshDataCajaMovimientos();
                refreshDataCajaDeAhorro();
            }
            else
            {
                MessageBox.Show("No se pudo retirar");
            }

        }
        //Depositar
        private void button14_Click(object sender, EventArgs e)
        {
            if(banco.Depositar(selectedCaja, Convert.ToInt32(textBox1.Text)))
            {
                MessageBox.Show("Se deposito correctamente: " + textBox1.Text);
                refreshDataCajaMovimientos();
                refreshDataCajaDeAhorro();
            }
            else
            {
                MessageBox.Show("No se pudoi depositar");
            }

        }
        //Transferir
        private void button13_Click(object sender, EventArgs e)
        {
            if(banco.Transferir(Convert.ToInt32(comboBox1.Text), Convert.ToInt32(comboBox2.Text), Convert.ToInt32(textBox2.Text)))
            {
                MessageBox.Show("Se transfirio correctamente $" + textBox2.Text + "A la cuenta: " + comboBox2.Text);
                refreshDataCajaMovimientos();
                refreshDataCajaDeAhorro();
            }
            else
            {
                MessageBox.Show("No se pudo tranferir");
            }

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

        // Seleccionador de Plazo Fijo
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedPLazoFijo = Convert.ToInt32(dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[0].Value);
        }

        //Agregar Plazo Fijo
        private void button4_Click(object sender, EventArgs e)
        {
            if (banco.AltaPlazoFijo(banco.usuarioActual, float.Parse(textBox3.Text), Convert.ToInt32(comboBox3.Text)))
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
            if (banco.BajaPlazoFijo(selectedPLazoFijo))
            {
                MessageBox.Show("Se elimino correctamente");
                refreshDataPlazoFijo();
            }
            else
            {
                MessageBox.Show("No se pudo eliminar");
            }
        }


        // PAGOS ////////////////////////////////////////////////////////////
        private void refreshDataPagos()
        {
            dataGridView3.Rows.Clear();
            dataGridView6.Rows.Clear();

            foreach (Pago p in banco.usuarioActual.pagos)
            {
                if(p.pagado == true) {
                     dataGridView3.Rows.Add(p.toArray());
                }
                else
                {
                    dataGridView6.Rows.Add(p.toArray());
                }
            }
        }

        //Seleccionar pago
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedPago= Convert.ToInt32(dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[0].Value);
        }

        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedPago = Convert.ToInt32(dataGridView6.Rows[dataGridView6.CurrentRow.Index].Cells[0].Value);
        }

        //Agregar Pago
        private void button5_Click(object sender, EventArgs e)
        {
            banco.AltaPago(banco.usuarioActual, textBox4.Text, float.Parse(textBox5.Text));
            refreshDataPagos();
        }

        //Pagar pago
        private void button7_Click(object sender, EventArgs e)
        {
            if(banco.ModificarPago(selectedPago, Convert.ToInt32(comboBox4.Text)))
            {
                MessageBox.Show("El pago se realizo correctamente");
                refreshDataPagos();
                refreshDataCajaDeAhorro();
                refreshDataTarjetasDeCredito();
            }
            else
            {
                MessageBox.Show("No se pudo realizar el pago");
            }
        }

        //Eliminar Pago
        private void button8_Click(object sender, EventArgs e)
        {
            if (banco.BajaPago(selectedPago))
            {
                MessageBox.Show("Se elimno el pago correctamente");
                refreshDataPagos();
            }
            else{
                MessageBox.Show("No se pudo eliminar el pago");
            }
        }

        // TARJETAS DE CREDITO ////////////////////////////////////////////////////////////
        private void refreshDataTarjetasDeCredito()
        {
            dataGridView4.Rows.Clear();

            foreach (TarjetaDeCredito t in banco.usuarioActual.tarjetas)
            {
                dataGridView4.Rows.Add(t.toArray());
            }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            banco.BajaTarjetaCredito(banco.usuarioActual);
            MessageBox.Show("Se elimino una nueva tarjeta de credito.");
            refreshDataTarjetasDeCredito();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            banco.AltaTarjetaCredito(banco.usuarioActual);
            MessageBox.Show("Se agrego una nueva tarjeta de credito.");
            refreshDataTarjetasDeCredito();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            banco.ModificarTarjetaDeCredito(banco.usuarioActual);
            MessageBox.Show("Se modifico una nueva tarjeta de credito.");
            refreshDataTarjetasDeCredito();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }



        private void button2_Click(object sender, EventArgs e)
        {

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

        private void label9_Click(object sender, EventArgs e)
        {

        }


    }
}
