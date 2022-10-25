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
    public partial class Form1 : Form
    {
        private Banco banco;
        Registrar hijoRegistrar;
        Main hijoMain;
        Login hijoLogin;
        ModificarDatosUsuario hijoModificarDatosUsuario;
        string Usuario;
        string Contraseña;
        bool Logued;
        public bool touched;

        public Form1()
        {
            InitializeComponent();
            banco = new Banco();
            banco.AltaUsuario("12345678", "12345678", "12345678", "12345678", "12345678", "12345678");//usuario de prueba
            Logued = false;
            hijoLogin = new Login(banco);
            hijoLogin.logued = false;
            hijoLogin.MdiParent = this;
            hijoLogin.TransfEvento += TransDelegado;
            // Abre ventana de registrar usuario
            hijoLogin.TransfEventoRegister += TransDelegadoRegister;
            hijoLogin.Show();
            touched = false;
        }

        // Metodo para recibir usuario y contraseña para validarlo con funcion de banco
        private void TransDelegado(string user, string pass)
        {
            this.Usuario = user;
            this.Contraseña = pass;

            if (banco.IniciarSesion(Usuario, Contraseña))
            {
                MessageBox.Show("Log in correcto, Usuario: " + Usuario);
                hijoLogin.Hide();
                hijoMain = new Main(new object[] { Usuario, banco });
                hijoMain.TransFModificarUsuario += TransDelegadoModificarUsuario;
                hijoMain.usuario = Usuario;
                hijoMain.MdiParent = this;
                /*
                banco.AltaCajaAhorro(banco.usuarioActual);
                banco.AltaCajaAhorro(banco.usuarioActual);
                banco.Depositar(banco.buscarCaja(1), 10000);
                banco.BajaCajaAhorro(1);
                banco.Depositar(banco.buscarCaja(2), 1000);
                */
                hijoMain.Show();
            }
            else
            {
                MessageBox.Show("Log in incorrecto, usuario no registrado.");
                hijoLogin.Show();
            }
        }

        // Metodo para Abrir form de registrar usuario
        private void TransDelegadoRegister(bool logued)
        {
            this.Logued = logued;
            if (this.Logued == false)
            {
                hijoLogin.Hide();
                hijoRegistrar = new Registrar(banco);
                hijoRegistrar.MdiParent = this;
                hijoRegistrar.Show();
                hijoRegistrar.TransEventoRegisterOk += TransDelegadoRegisterOK;
            }
        }

        // Metodo para cerrar el form de registro cuando se registra un usuario
        private void TransDelegadoRegisterOK(bool confirmacionRegistro)
        {
            if (confirmacionRegistro == true)
            {
                hijoRegistrar.Hide();
                hijoLogin.Show();
            }
        }
    
        // Metodo para abrir el form de modificar usuario
        private void TransDelegadoModificarUsuario(bool modificar)
        {
            if(modificar == true)
            {
                hijoMain.Hide();
                hijoModificarDatosUsuario = new ModificarDatosUsuario(banco);
                hijoModificarDatosUsuario.MdiParent = this;
                hijoModificarDatosUsuario.Show();
                hijoModificarDatosUsuario.TransfEventoModificarUsuarioOk += TransDelegadoModificarUsuarioOk;
                
            }
        }

        // Metodo para cerrar el form de modificar usuario
        private void TransDelegadoModificarUsuarioOk(bool confirmacionModificar)
        {
            if (confirmacionModificar == true)
            {
                hijoModificarDatosUsuario.Close();
                hijoMain.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
