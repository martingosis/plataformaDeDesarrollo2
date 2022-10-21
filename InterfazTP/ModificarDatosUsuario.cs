﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static InterfazTP.Main;

namespace InterfazTP
{
    public partial class ModificarDatosUsuario : Form
    {
        Banco banco;
        bool confirmacionModificacion;
        public TransDelegadoModificarUsuarioOk TransfEventoModificarUsuarioOk;

        public ModificarDatosUsuario(Banco banco)
        {
            InitializeComponent();
            this.banco = banco; 
        }

        // Boton confirmar modificar datos
        private void button1_Click(object sender, EventArgs e)
        {
            if (banco.ModificarUsuario(textBox5.Text, textBox6.Text, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text))
            {
                MessageBox.Show("Usuario Modificado con exito!");
                confirmacionModificacion = true;
                this.TransfEventoModificarUsuarioOk(confirmacionModificacion);
            }
            else
            {
                MessageBox.Show("No se pudo modificar usuario");
            }
        }

        public delegate void TransDelegadoModificarUsuarioOk(bool cofirmacionModificacion);

        private void ModificarVolver_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 frm = new Form1();
            frm.Show();
        }
    }
}
