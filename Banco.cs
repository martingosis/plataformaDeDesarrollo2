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

        public bool BajaCajaAhorro(int id)
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
                            return true;
                        }
                    }
                }
            }
            return false;
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

        // CLAUDIO
        public void AltaMovimiento(CajaDeAhorro c, string detalle, float monto)
        {
            c.agregarMovimiento(new Movimiento(c, detalle, monto));
        }

        public void AltaPago(Usuario usuario, string nombre, float monto)
        {
            Pago p = new Pago(usuario, nombre, monto);
            pagos.Add(p);
            usuarioActual.pagos.Add(p);
        }

        public bool BajaPago(int id)
        {
            foreach (var p in pagos)
            {
                if (p.id == id && p.pagado == true)
                {
                    pagos.Remove(p);
                    usuarioActual.pagos.Remove(p);
                    return true;
                }
            }
            return false;
        }

        public bool ModificarPago(int id, int identificador)
        {
            foreach (var p in pagos)
            {
                if (p.id == id && p.pagado == false)
                {
                    foreach(CajaDeAhorro c in usuarioActual.cajas)
                    {
                        if (c.cbu == identificador && c.saldo >= p.monto)
                        {
                            p.pagado = true;
                            c.saldo -= p.monto;
                            c.agregarMovimiento(new Movimiento(c, "Pago", p.monto));
                            return true;
                        }
                    }
                    foreach (TarjetaDeCredito t in usuarioActual.tarjetas)
                    {
                        if (t.numero == identificador && (t.limite - t.consumos) >= p.monto)
                        {
                            p.pagado = true;
                            t.consumos += p.monto;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool AltaPlazoFijo(Usuario u, float monto, int cbuDestino)
        {
            foreach(CajaDeAhorro c in caja)
            {
                if(c.cbu == cbuDestino && c.saldo >= monto && monto >= 1000)
                {
                    PlazoFijo pfj = new PlazoFijo(u, monto);
                    usuarioActual.pf.Add(pfj);
                    plazosFijos.Add(pfj);
                    c.saldo -= pfj.monto;    
                    return true;
                }
            }
            return false;
        }

        public bool BajaPlazoFijo(int id)
        {
            foreach (var p in plazosFijos)
            {
                if (p.id == id)
                {
                    if (p.pagado == true && (DateTime.Now - p.fechaFin).TotalDays > 30)
                    {
                        usuarioActual.pf.Remove(p);
                        plazosFijos.Remove(p);
                        return true;
                    }
                }
            }
            return false;
        }

        public void cobrarPlazoFijo(int plazoFijoID)
        {
            foreach (PlazoFijo pf in plazosFijos)
            {
                if(pf.id == plazoFijoID)
                {
                    usuarioActual.cajas.First().saldo = pf.monto + (pf.monto*(pf.getTasa() / 365));
                    pf.pagado = true;
                }
            }
        }

        // LUCAS
        public void AltaTarjetaCredito(Usuario usuarioActual)
        {
            if (usuarioActual != null)
            {
                //Datos MOCKEADOS
                TarjetaDeCredito tarjetaDeCredito = new TarjetaDeCredito();
                tarjetaDeCredito.titular = usuarioActual;
                tarjetaDeCredito.limite = 50000;
                tarjetaDeCredito.id = usuarioActual.id;
                tarjetaDeCredito.codigoV = new DateOnly(2020, 02, 01);
                tarjetaDeCredito.consumos = 0;
                usuarioActual.tarjeta.Add(tarjetaDeCredito);
            }
        }

        public void BajaTarjetaCredito(Usuario usuarioActual)
        {
            if (usuarioActual != null && usuarioActual.tarjeta.Count != 0)
            {
                foreach (var t in usuarioActual.tarjeta)
                {
                    if (t.consumos == 0)
                    {
                        Console.WriteLine("Tarjeta eliminada exitosamente");
                        usuarioActual.tarjeta.Remove(t);
                    }
                    else
                    {
                        Console.WriteLine("No se puede eliminar su tarjeta de credito porque tiene consumos pendientes");
                    }
                }
            }

        }

        public void ModificarTarjetaDeCredito(Usuario usuarioActual)
        {
            if (usuarioActual != null)
            {
                foreach (var t in usuarioActual.tarjeta)
                {
                    if (usuarioActual.id == t.id)
                    {
                        t.limite = 0;
                    }
                }
            }
        }




        public List<TarjetaDeCredito> MostrarTarjetasDeCredito()
        {
            return usuarioActual.tarjeta;
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
                        AltaMovimiento(c, "Deposito", monto);
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
                        AltaMovimiento(c, "Retiro", monto);
                        return true;
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
                            AltaMovimiento(ca, "Transferencia Recibida", monto);
                            AltaMovimiento(c, "Transferencia Enviada", monto);
                            return true;
                        }
                    }
                }
            }
         return false;
        }


        // MOSTRAR DATOS - Nico

        public List<CajaDeAhorro> MostrarCajasDeAhorro()
        {
            return this.usuarioActual.cajas.ToList();
        }
        public List<Movimiento> MostrarMovimientos(int cajaID)
        {
            if (usuarioActual != null)
            {
                foreach (CajaDeAhorro c in usuarioActual.cajas)
                {
                    if (cajaID == c.id)
                    {
                        return c.movimientos.ToList();
                    }
                }
            }
            return null;
        }
        public List<Pago> MostrarPagos()
        {
            return usuarioActual.pagos.ToList();
        }

        public List<PlazoFijo> MostrarPlazoFijos()
        {
            return usuarioActual.pf.ToList();
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
    }
}
