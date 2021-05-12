using API.Facturacion.DTO.Clases;
using API.Facturacion.DTO.Peticion;
using API.Facturacion.DTO.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Facturacion.Servicios
{
    public class FacturaServicio
    {
        public RespuestaFacturaConsultar Obtener(int facId)
        {
            var resultado = new RespuestaFacturaConsultar();
            try
            {
                var dataContext = new DAL.Facturacion.Models.FacturacionContext();
                var facturas = dataContext.Factura.Where(f => f.Activo == true && f.Id == facId).Select(factura => new Factura
                {
                    Id = factura.Id,
                    Numero = factura.Numero,
                    FechaExpedicion = factura.FechaExpedicion,
                    ValorTotal = factura.ListaFacturaDetalles.Sum(fd => fd.Cantidad * fd.PrecioUnitario),
                    Cliente = new Cliente
                    {
                        Id = factura.Cliente.Id,
                        TidId = factura.Cliente.TidId,
                        Identificacion = factura.Cliente.Identificacion,
                        Nombres = factura.Cliente.Nombres,
                        Apellidos = factura.Cliente.Apellidos,
                        FechaNacimiento = factura.Cliente.FechaNacimiento,
                        CorreoElectronico = factura.Cliente.CorreoElectronico ?? ""
                    },
                }).Take(1).ToList();

                foreach (var factura in facturas)
                {
                    factura.ListaFacturaDetalles = dataContext.FacturaDetalle.Where(fde => fde.FacId == factura.Id)
                    .Select(detalle => new FacturaDetalle
                    {
                        Producto = new Producto
                        {
                            Id = detalle.Producto.Id,
                            Codigo = detalle.Producto.Codigo,
                            Nombre = detalle.Producto.Nombre
                        },
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario
                    }).ToList();
                }

                resultado.ListaFacturas = facturas;

                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            catch (Exception)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = "Ocurrio un error al consultar la factura indicada.";
            }
            return resultado;
        }

        public RespuestaFacturaConsultar Listar()
        {
            var resultado = new RespuestaFacturaConsultar();
            try
            {

                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            catch (Exception)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = "Ocurrio un error al consultar las facturas.";
            }
            return resultado;
        }

        public RespuestaFacturaConsultar Consultar(string numero)
        {
            var resultado = new RespuestaFacturaConsultar();
            try
            {
                var dataContext = new DAL.Facturacion.Models.FacturacionContext();
                var facturas = dataContext.Factura.Where(f => f.Activo == true && f.Numero.Contains(numero)).Select(factura => new Factura
                {
                    Id = factura.Id,
                    Numero = factura.Numero,
                    FechaExpedicion = factura.FechaExpedicion,
                    ValorTotal = factura.ListaFacturaDetalles.Sum(fd => fd.Cantidad * fd.PrecioUnitario),
                    Cliente = new Cliente
                    {
                        Id = factura.Cliente.Id,
                        Identificacion = factura.Cliente.Identificacion,
                        Nombres = factura.Cliente.Nombres,
                        Apellidos = factura.Cliente.Apellidos,
                    },
                }).ToList();

                resultado.ListaFacturas = facturas;

                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            catch (Exception)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = "Ocurrio un error al consultar las facturas.";
            }
            return resultado;
        }

        public RespuestaFacturaCrear Crear(PeticionFacturaCrear datosFacturaCrear)
        {
            var resultado = new RespuestaFacturaCrear();
            try
            {
                if (datosFacturaCrear == null)
                {
                    throw new Exception("El objeto llegó nulo.");
                }
                else if (datosFacturaCrear.Factura == null)
                {
                    throw new Exception("No se agrego la factura a crear.");
                }
                var respuestaValidacion = ValidarFactura(datosFacturaCrear.Factura);

                if (respuestaValidacion.Estado == EstadoPeticion.ERROR)
                {
                    throw new Exception(respuestaValidacion.Mensaje);
                }

                DateTime fechaCreacion = DateTime.Now;

                var dataContext = new DAL.Facturacion.Models.FacturacionContext();

                var dbFactura = new DAL.Facturacion.Models.Factura();

                var factura = datosFacturaCrear.Factura;

                var cliente = factura.Cliente;
                if (cliente.Id == 0)
                {
                    var peticionClienteCrear = new PeticionClienteCrear();

                    peticionClienteCrear.Cliente = cliente;

                    var respuesta = new ClienteServicio().Crear(peticionClienteCrear);

                    if (respuesta.Estado == EstadoPeticion.ERROR)
                    {
                        throw new Exception(respuesta.Mensaje);
                    }
                    else
                    {
                        cliente.Id = respuesta.Cliente.Id;
                    }
                }

                dbFactura.CliId = cliente.Id;
                dbFactura.FechaExpedicion = factura.FechaExpedicion;
                dbFactura.Numero = "";

                dbFactura.FechaCreacion = fechaCreacion;
                dbFactura.FechaActualizacion = fechaCreacion;
                dbFactura.Activo = true;

                var dbProducto = new DAL.Facturacion.Models.Producto();

                foreach (var detalleProducto in factura.ListaFacturaDetalles)
                {
                    var dbFacturaDetalle = new DAL.Facturacion.Models.FacturaDetalle();

                    dbFacturaDetalle.ProId = detalleProducto.Producto.Id;
                    dbFacturaDetalle.Cantidad = detalleProducto.Cantidad;
                    dbFacturaDetalle.PrecioUnitario = detalleProducto.PrecioUnitario;

                    dbFacturaDetalle.FechaCreacion = fechaCreacion;
                    dbFacturaDetalle.FechaActualizacion = fechaCreacion;
                    dbFacturaDetalle.Activo = true;


                    dbProducto = dataContext.Producto.FirstOrDefault(p => p.Activo == true && p.Id == detalleProducto.Producto.Id);
                    if (dbProducto == null)
                    {
                        throw new Exception($"El producto {detalleProducto.Producto.Nombre} con el código {detalleProducto.Producto.Codigo} NO existe o NO está disponible.");
                    }
                    else if ((dbProducto.CantidadInventario - 5) < detalleProducto.Cantidad)
                    {
                        throw new Exception($"El producto {detalleProducto.Producto.Nombre} con el código {detalleProducto.Producto.Codigo} sólo tiene {dbProducto.CantidadInventario} unidades en existencia.");
                    }

                    dbProducto.CantidadInventario -= detalleProducto.Cantidad;

                    dbFactura.ListaFacturaDetalles.Add(dbFacturaDetalle);
                }

                dataContext.Factura.Add(dbFactura);
                dataContext.SaveChanges();

                factura.Id = dbFactura.Id;

                resultado.Factura = factura;
                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            catch (Exception ex)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = $"Ocurrio un error al crear la factura. {ex.Message}";
            }
            return resultado;
        }

        public RespuestaFacturaCrear Actualizar(int facId, PeticionFacturaCrear datosFacturaCrear)
        {
            var resultado = new RespuestaFacturaCrear();
            try
            {
                if (datosFacturaCrear == null)
                {
                    throw new Exception("El objeto llegó nulo.");
                }
                else if (datosFacturaCrear.Factura == null)
                {
                    throw new Exception("No se agrego la factura a actualizar.");
                }

                var respuestaValidacion = ValidarFactura(datosFacturaCrear.Factura);

                if (respuestaValidacion.Estado == EstadoPeticion.ERROR)
                {
                    throw new Exception(respuestaValidacion.Mensaje);
                }

                DateTime fechaActualizacion = DateTime.Now;

                var dataContext = new DAL.Facturacion.Models.FacturacionContext();

                var dbFactura = dataContext.Factura.SingleOrDefault(f=>f.Id == datosFacturaCrear.Factura.Id);

                if (dbFactura == null)
                {
                    throw new Exception("La factura indicada NO existe o no está activo.");
                }

                var factura = datosFacturaCrear.Factura;

                var cliente = factura.Cliente;
                if (cliente.Id != 0)
                {
                    var peticionClienteCrear = new PeticionClienteCrear();

                    peticionClienteCrear.Cliente = cliente;

                    var respuesta = new ClienteServicio().Actualizar(peticionClienteCrear);

                    if (respuesta.Estado == EstadoPeticion.ERROR)
                    {
                        throw new Exception(respuesta.Mensaje);
                    }
                    else
                    {
                        cliente.Id = respuesta.Cliente.Id;
                    }
                }

                dbFactura.CliId = cliente.Id;
                dbFactura.FechaExpedicion = factura.FechaExpedicion;
                dbFactura.Numero = "";

                dbFactura.FechaActualizacion = fechaActualizacion;
                dbFactura.Activo = true;

                foreach (var detalleProducto in factura.ListaFacturaDetalles)
                {
                    var dbFacturaDetalle = new DAL.Facturacion.Models.FacturaDetalle();
                    dbFacturaDetalle.FechaCreacion = fechaActualizacion;


                    var dbFacturaDetalleExistente = dbFactura.ListaFacturaDetalles.SingleOrDefault(a => a.Activo == true && a.Id != 0 && a.ProId == detalleProducto.Producto.Id);
                    if (dbFacturaDetalleExistente != null)
                    {
                        dbFacturaDetalle = dbFacturaDetalleExistente;
                    }

                    dbFacturaDetalle.ProId = detalleProducto.Producto.Id;
                    dbFacturaDetalle.Cantidad = detalleProducto.Cantidad;
                    dbFacturaDetalle.PrecioUnitario = detalleProducto.PrecioUnitario;

                    dbFacturaDetalle.FechaActualizacion = fechaActualizacion;
                    dbFacturaDetalle.Activo = true;

                    dbFactura.ListaFacturaDetalles.Add(dbFacturaDetalle);
                }

                dataContext.SaveChanges();

                factura.Id = dbFactura.Id;

                resultado.Factura = factura;
                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            catch (Exception ex)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = $"Ocurrio un error al actualizar la factura. {ex.Message}";
            }
            return resultado;
        }

        public RespuestaFacturaEliminar Eliminar(int facId)
        {
            var resultado = new RespuestaFacturaEliminar();
            try
            {
                var dataContext = new DAL.Facturacion.Models.FacturacionContext();
                var factura = dataContext.Factura.SingleOrDefault(f => f.Id == facId);

                if (factura != null)
                {
                    factura.Activo = false;
                    dataContext.SaveChanges();

                    resultado.Estado = EstadoPeticion.OK;
                    resultado.Mensaje = "";
                }
                else
                {
                    resultado.Estado = EstadoPeticion.ERROR;
                    resultado.Mensaje = "Ocurrio un error al consultar la factura indicada.";
                }
            }
            catch (Exception)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = "Ocurrio un error al consultar la factura indicada.";
            }
            return resultado;
        }

        internal Respuesta ValidarFactura(Factura factura)
        {
            var resultado = new Respuesta();
            bool error = false;
            List<string> mensajes = new List<string>();
            if (factura.Cliente == null)
            {
                error = true;
                mensajes.Add("NO se ingresó un cliente en la factura.");
            }
            else
            {
                new ClienteServicio().ValidarCliente(factura.Cliente);

            }

            if (factura.ListaFacturaDetalles == null || !factura.ListaFacturaDetalles.Any())
            {
                error = true;
                mensajes.Add("NO se agregaron productos en la factura.");
            }
            else
            {
                if (factura.Id == 0)
                {
                    var respuesta = ValidarDetallesFacturaCrear(factura);
                    if (respuesta.Estado == EstadoPeticion.ERROR)
                    {
                        error = true;
                        mensajes.Add(respuesta.Mensaje);
                    }
                }
                else
                {
                    var respuesta = ValidarDetallesFacturaActualizar(factura);
                    if (respuesta.Estado == EstadoPeticion.ERROR)
                    {
                        error = true;
                        mensajes.Add(respuesta.Mensaje);
                    }
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

        internal Respuesta ValidarDetallesFacturaCrear(Factura factura)
        {
            var resultado = new Respuesta();
            bool error = false;
            List<string> mensajes = new List<string>();

            var dataContext = new DAL.Facturacion.Models.FacturacionContext();

            foreach (var detalle in factura.ListaFacturaDetalles)
            {
                var producto = dataContext.Producto.SingleOrDefault(p => p.Activo == true && p.Id == detalle.Producto.Id);
                if (producto == null)
                {
                    error = true;
                    mensajes.Add($"El producto {detalle.Producto.Nombre} con el código {detalle.Producto.Codigo} NO existe o NO está disponible.");
                }
                else if ((producto.CantidadInventario - 5) < detalle.Cantidad)
                {
                    error = true;
                    mensajes.Add($"El producto {detalle.Producto.Nombre} con el código {detalle.Producto.Codigo} sólo tiene {producto.CantidadInventario} unidades en existencia.");
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
        internal Respuesta ValidarDetallesFacturaActualizar(Factura factura)
        {
            var resultado = new Respuesta();
            bool error = false;
            List<string> mensajes = new List<string>();

            var dataContext = new DAL.Facturacion.Models.FacturacionContext();

            foreach (var detalle in factura.ListaFacturaDetalles)
            {
                var producto = dataContext.Producto.SingleOrDefault(p => p.Activo == true && p.Id == detalle.Producto.Id);
                if (producto == null)
                {
                    error = true;
                    mensajes.Add($"El producto {detalle.Producto.Nombre} con el código {detalle.Producto.Codigo} NO existe o NO está disponible.");
                }
                else 
                {
                    int cantidadFacturada = 0;
                    var dbFacturaDetalle = dataContext.FacturaDetalle.SingleOrDefault(a => a.Activo == true && a.FacId == factura.Id && a.ProId == detalle.Producto.Id);
                    if (dbFacturaDetalle != null)
                    {
                        cantidadFacturada = dbFacturaDetalle.Cantidad;
                    }
                    if ((producto.CantidadInventario - 5) < (detalle.Cantidad - cantidadFacturada))
                    {
                        error = true;
                        mensajes.Add($"El producto {detalle.Producto.Nombre} con el código {detalle.Producto.Codigo} sólo tiene {producto.CantidadInventario} unidades en existencia.");
                    }
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
