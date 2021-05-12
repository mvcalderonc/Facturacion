using API.Facturacion.DTO.Clases;
using API.Facturacion.DTO.Peticion;
using API.Facturacion.DTO.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Facturacion.Servicios
{
    public class ClienteServicio
    {
        public RespuestaClienteConsultar Consultar(int tidId, string identificacion)
        {
            var resultado = new RespuestaClienteConsultar();
            try
            {
                var dataContext = new DAL.Facturacion.Models.FacturacionContext();
                var clientes = dataContext.Cliente.Where(c => c.Activo == true && c.TidId == tidId && c.Identificacion == identificacion);

                if (clientes.Any())
                {
                    if (clientes.Count() == 1)
                    {
                        resultado.ListaClientes = clientes.Select(cliente => new Cliente
                        {
                            Id = cliente.Id,
                            TidId = cliente.TidId,
                            Identificacion = cliente.Identificacion,
                            Nombres = cliente.Nombres,
                            Apellidos = cliente.Apellidos,
                            FechaNacimiento = cliente.FechaNacimiento,
                            CorreoElectronico = cliente.CorreoElectronico
                        }).ToList();

                        resultado.Estado = EstadoPeticion.OK;
                        resultado.Mensaje = "";
                    }
                    else
                    {
                        resultado.Estado = EstadoPeticion.ERROR;
                        resultado.Mensaje = "Ocurrio un error, se encontró más de un cliente con el mismo tipo y número de identificación.";
                    }
                }
                else
                {
                    resultado.Estado = EstadoPeticion.OK;
                    resultado.Mensaje = "NO se encontró un cliente con el tipo y número de identificación indicado.";
                }
            }
            catch (Exception)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = "Ocurrio un error al consultar los tipos de identificación.";
            }
            return resultado;
        }

        public RespuestaClienteCrear Crear(PeticionClienteCrear datosClienteCrear)
        {
            var resultado = new RespuestaClienteCrear();
            try
            {
                DateTime fechaCreacion = DateTime.Now;

                var dataContext = new DAL.Facturacion.Models.FacturacionContext();

                var cliente = datosClienteCrear.Cliente;

                var dbCliente = new DAL.Facturacion.Models.Cliente();
                dbCliente.TidId = cliente.TidId;
                dbCliente.Identificacion = cliente.Identificacion;
                dbCliente.Nombres = cliente.Nombres;
                dbCliente.Apellidos = cliente.Apellidos;
                dbCliente.CorreoElectronico = cliente.CorreoElectronico;
                dbCliente.FechaNacimiento = cliente.FechaNacimiento;

                dbCliente.FechaCreacion = fechaCreacion;
                dbCliente.FechaActualizacion = fechaCreacion;
                dbCliente.Activo = true;

                dataContext.Cliente.Add(dbCliente);
                dataContext.SaveChanges();


                cliente.Id = dbCliente.Id;

                resultado.Cliente = cliente;
                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            catch (Exception ex)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = $"Ocurrio un error al crear el cliente. {ex.Message}";
            }
            return resultado;
        }

        public RespuestaClienteCrear Actualizar(PeticionClienteCrear datosClienteCrear)
        {
            var resultado = new RespuestaClienteCrear();
            try
            {
                DateTime fechaCreacion = DateTime.Now;

                var dataContext = new DAL.Facturacion.Models.FacturacionContext();

                var cliente = datosClienteCrear.Cliente;

                var dbCliente = dataContext.Cliente.SingleOrDefault(c=>c.Activo == true && c.Id == cliente.Id);

                if (dbCliente == null)
                {
                    throw new Exception("El cliente indicado NO existe o no está activo.");
                }

                dbCliente.TidId = cliente.TidId;
                dbCliente.Identificacion = cliente.Identificacion;
                dbCliente.Nombres = cliente.Nombres;
                dbCliente.Apellidos = cliente.Apellidos;
                dbCliente.CorreoElectronico = cliente.CorreoElectronico;
                dbCliente.FechaNacimiento = cliente.FechaNacimiento;

                dbCliente.FechaCreacion = fechaCreacion;
                dbCliente.FechaActualizacion = fechaCreacion;
                dbCliente.Activo = true;

                dataContext.SaveChanges();


                cliente.Id = dbCliente.Id;

                resultado.Cliente = cliente;
                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            catch (Exception ex)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = $"Ocurrio un error al actualizar el cliente. {ex.Message}";
            }
            return resultado;
        }

        internal Respuesta ValidarCliente(Cliente cliente)
        {
            var resultado = new Respuesta();
            bool error = false;
            List<string> mensajes = new List<string>();

            var dataContext = new DAL.Facturacion.Models.FacturacionContext();

            var tipoIdentificacion = dataContext.TipoIdentificacion.SingleOrDefault(t => t.Id == cliente.TidId);
            if (tipoIdentificacion == null)
            {
                error = true;
                mensajes.Add("NO se indicó un tipo de identificación para el cliente.");
            }
            if (string.IsNullOrEmpty((cliente.Identificacion ?? "").Trim()))
            {
                error = true;
                mensajes.Add("NO se ingresó un número de identificación para el cliente.");
            }
            if (string.IsNullOrEmpty((cliente.Nombres ?? "").Trim()))
            {
                error = true;
                mensajes.Add("NO se ingresó un nombre para el cliente.");
            }

            if (tipoIdentificacion != null && tipoIdentificacion.Codigo == "CC")
            {
                if (string.IsNullOrEmpty((cliente.Apellidos ?? "").Trim()))
                {
                    error = true;
                    mensajes.Add("NO se ingresó un apellido para el cliente.");
                }
                if (cliente.FechaNacimiento == null)
                {
                    error = true;
                    mensajes.Add("NO se ingresó un nombre para el cliente.");
                }
            }

            if (!string.IsNullOrEmpty((cliente.CorreoElectronico ?? "").Trim()))
            {
                String expresion;
                expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(cliente.CorreoElectronico, expresion))
                {
                    if (!(Regex.Replace(cliente.CorreoElectronico, expresion, String.Empty).Length == 0))
                    {
                        error = true;
                        mensajes.Add("NO se ingresó correo electrónico válido para el cliente.");
                    }
                }
                else
                {
                    error = true;
                    mensajes.Add("NO se ingresó correo electrónico válido para el cliente.");
                }

                
            }

            if (error)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = "";
                var separador = "";
                foreach (var mensaje in mensajes)
                {
                    resultado.Mensaje = $"{resultado.Mensaje}{separador}{mensaje}";
                    separador = "<br />";
                }
            }
            else
            {
                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            return resultado;
        }
    }
}
