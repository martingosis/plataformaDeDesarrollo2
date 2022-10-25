﻿using Microsoft.VisualBasic.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace InterfazTP
{
    public class Banco
    {
        public List<Usuario> usuario { get; set; }
        private List<CajaDeAhorro> caja { get; set; }
        private List<PlazoFijo> plazosFijos { get; set; }
        private List<TarjetaDeCredito> tarjetas { get; set; }
        private List<Pago> pagos { get; set; }
        private List<Movimiento> movimientos { get; set; }
        public Usuario usuarioActual { get; set; }
        public int cbu = 1000;

        public Banco()
        {
            pagos = new List<Pago>();
            tarjetas = new List<TarjetaDeCredito>();
            movimientos = new List<Movimiento>();
            plazosFijos = new List<PlazoFijo>();
            caja = new List<CajaDeAhorro>();
            usuario = new List<Usuario>();
        }

        // Kevin
        public bool AltaUsuario(string user, string password, string nombre, string apellido, string dni, string email)
        {
            if (password.Length < 8 || user.Length < 8)
            {
                MessageBox.Show("Usuario y Contraseña deben tener minimo 8 caracteres.");
                return false;
            }
            else
            {
                Usuario nuevoUsuario = new Usuario();
                nuevoUsuario.usuarioLogin = user;
                nuevoUsuario.password = password;
                nuevoUsuario.nombre = nombre;
                nuevoUsuario.apellido = apellido;
                nuevoUsuario.dni = dni;
                nuevoUsuario.email = email;

                usuario.Add(nuevoUsuario);
                return true;
            }
        }

        public bool ModificarUsuario(string user, string password, string nombre, string apellido, string dni, string email)
        {
            bool result = false;

            foreach (var usuario in usuario)
            {
                if (usuario.usuarioLogin == usuarioActual.usuarioLogin)
                {

                    if (nombre == "" || apellido == "" || dni == "" || email == "")
                    {
                        result = false;
                    }
                    else
                    {
                        if (user.Length < 8 || password.Length < 8)
                        {
                            MessageBox.Show("Usuario y contraseña deben tener minimo 8 caracteres.");
                            result = false;
                        }
                        else
                        {
                            usuario.nombre = nombre;
                            usuario.apellido = apellido;
                            usuario.email = email;
                            usuario.usuarioLogin = user;
                            usuario.password = password;

                            result = true;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No registrado");
                    result = false;
                }
            }
            return result;
        }

        public void EliminarUsuario()
        {
            // Copiar codigo de git
        }

        public void AltaCajaAhorro(Usuario usuarioActual)
        {

            if (usuarioActual != null)
            {
                // Usuario
                CajaDeAhorro nuevaCajaAhorro = new CajaDeAhorro();
                nuevaCajaAhorro.saldo = 0;
                nuevaCajaAhorro.cbu = cbu;
                nuevaCajaAhorro.titular.Add(usuarioActual);
                usuarioActual.cajas.Add(nuevaCajaAhorro);


                // Banco
                caja.Add(nuevaCajaAhorro);
                cbu++;

            }

        }

        public void BajaCajaAhorro(int id)
        {
            if (usuarioActual != null)
            {
                foreach (var c in usuarioActual.obtenerCajas())
                {
                    if (c.id == id)
                    {
                        if (c.saldo == 0)
                        {
                            usuarioActual.cajas.Remove(c);
                            caja.Remove(c);
                        }
                        else
                        {
                            Console.WriteLine("La caja no se puede eliminar si cuenta con saldo.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No tiene cajas de ahorro asociadas.");
                    }
                }
            }
            else
            {
                Console.WriteLine("No tiene cajas de ahorro.");
            }

        }

        public void ModificarCajaAhorro(int id)
        {
            foreach (var c in usuarioActual.cajas)
            {
                if (c.id == id)
                {
                    c.id = 0;
                    c.cbu = 0;
                }
                else
                {
                    Console.WriteLine("No tiene caja ahorro!");
                }
            }
        }

        /* public bool IniciarSesion(string usuario, string contraseña)
         {
             bool encontre = false;

             foreach (Usuario usuarioInd in this.usuario)
             {


                 if (usuarioInd.usuarioLogin == usuario && usuarioInd.password == contraseña)
                 {
                     encontre = true;
                     usuarioActual = usuarioInd;
                 }

             }
             return encontre;
         }*/

        // CLAUDIO
        public void AltaMovimiento(Movimiento mov, CajaDeAhorro entrada)
        {
            movimientos.Add(mov);
            entrada.movimientos.Add(mov);
        }

        public void AltaPago(Pago pg, Usuario usuarioA)
        {
            usuarioA.pagos.Add(pg);
            // pg.user = usuarioA;  
        }

        public void BajaPago(int id)
        {
            foreach (var p in pagos)
            {
                if (p.id == id)
                {
                    pagos.Remove(p);
                    usuarioActual.pagos.Remove(p);
                }
                else
                {
                    Console.WriteLine("salida en 163");
                }
            }
        }

        public void ModificarPago(int id)
        {
            foreach (var p in pagos)
            {
                if (p.id == (int)id)
                {
                    p.pagado = true;
                }
            }
        }

        public void AltaPlazoFijo(PlazoFijo pfj, Usuario usuario)
        {
            usuario.pf.Add(pfj);
            plazosFijos.Add(pfj);
        }

        public void BajaPlazoFijo(int id)
        {
            foreach (var p in plazosFijos)
            {
                if (p.id == (int)id)
                {
                    if (p.pagado == true && p.fechaFin > DateTime.Now)
                    {
                        usuarioActual.pf.Remove(p);
                        plazosFijos.Remove(p);
                    }
                    else
                    {
                        Console.WriteLine("salida en 208");
                    }
                }
            }
        }

        public void cobrarPlazoFijo(int plazoFijoID)
        {
            foreach (PlazoFijo pf in plazosFijos)
            {
                if(pf.id == plazoFijoID)
                {
                    //usuarioActual.cajas.First().saldo;
                }
            }
        }

        // AUGUSTO

        public void AltaTarjetaCredito(Usuario usuario)
        {

        }

        public void BajaTarjetaCredito()
        {

        }

        public void ModificarTarjetaDeCredito()
        {

        }

        // Martin

        public bool IniciarSesion(string usuario, string contrasenia)
        {
            bool encontrado = false;
            try
            {
                foreach (Usuario usuarioInd in this.usuario)
                {

                    if (usuarioInd.nombre == usuario)
                    {

                        if (usuarioInd.bloqueado == false)
                        {

                            while (usuarioInd.intentosFallidos < 4 && encontrado == false)
                            {


                                // agregar verificacion usuario
                                if (usuarioInd.password == contrasenia)
                                {
                                    usuarioActual = usuarioInd;
                                    usuarioInd.intentosFallidos = 0;
                                    encontrado = true;
                                }
                                else
                                {
                                    usuarioInd.intentosFallidos++;

                                    if (usuarioInd.intentosFallidos == 4)
                                    {

                                        usuarioInd.bloqueado = true;
                                    }

                                }
                            }
                        }

                    }

                }


            }
            catch (Exception i)
            {

                Console.WriteLine("error de " + i);
            }
        return encontrado;
        }

        public void CerrarSesion()
        {
            usuarioActual = null;
        }

        public bool Depositar(int idCaja, float monto)
        {
            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro c in usuarioActual.cajas)
                {

                    if (c.id == idCaja)
                    {
                        c.saldo = monto + c.saldo;
                        c.agregarMovimiento(new Movimiento(c, "Deposito", monto));
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Retirar(int CajaID, float monto)
        {
            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro c in usuarioActual.cajas)
                {
                    if (c.saldo >= monto && c.id == CajaID)
                    {
                        c.saldo = c.saldo - monto;
                        c.agregarMovimiento(new Movimiento(c, "Retiro", monto));
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Fondos insuficientes");
                        return false;
                    }
                }
            }
            return false;
        }

        public bool Transferir(int cajaOrigenCBU, int cajaDestinoCBU, float monto)
        {
            foreach (CajaDeAhorro c in usuarioActual.cajas)
            {
                if (cajaOrigenCBU == c.cbu && c.saldo >= monto && cajaOrigenCBU != cajaDestinoCBU)
                {
                    c.saldo -= monto;
                    foreach (CajaDeAhorro ca in usuarioActual.cajas)
                    {
                        if (cajaDestinoCBU == ca.cbu)
                        {
                            ca.saldo += monto;
                            ca.agregarMovimiento(new Movimiento(ca, "Transferencia Recibida", monto));
                            c.agregarMovimiento(new Movimiento(c, "Transferencia Enviada", monto));
                            return true;
                        }
                    }
                }
            }
         return false;
        }


        // MOSTRAR DATOS - Nico
        public List<Pago> MostrarPagos()
        {
            // Verificar el devolver con toList
            return usuarioActual.pagos;
        }

        public List<PlazoFijo> MostrarPlazoFijos()
        {
            return usuarioActual.pf;
        }

        public List<Movimiento> MostrarMovimientos(int cajaID)
        {
            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro cajas in usuarioActual.cajas)
                {
                    if (cajaID == cajas.id)
                    {
                        return cajas.movimientos.ToList();
                    }
                }
            }
            else
            {
                Console.WriteLine("error en la lista 135");
            }
            return null;
        }

        public List<Movimiento> BuscarMovimiento(CajaDeAhorro cajas, String detalle, DateTime fecha, float monto)
        {
            List<Movimiento> nuevo = new List<Movimiento>();

            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro usuarioInd in usuarioActual.cajas)
                {
                    if (usuarioInd.id == cajas.id)
                    {
                        foreach (Movimiento movi in usuarioInd.movimientos)
                        {
                            if ((movi.detalle == detalle) || (movi.fecha == fecha) || (movi.monto == monto))
                            {
                                nuevo.Add(movi);
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("error en la lista 135");
            }
            return nuevo;
        }

        public CajaDeAhorro buscarCaja(int id)
        {
            CajaDeAhorro res = null;
            foreach(CajaDeAhorro c in caja)
            {
                if(c.id == id)
                {
                    res = c;
                }
            }
            return res;
        }

        public List<CajaDeAhorro> MostrarCajasDeAhorro()
        {
            return this.usuarioActual.cajas.ToList();
        }

        public List<Movimiento> MostrarMovimientos()
        {
            return this.movimientos.ToList();
        }
    }
}