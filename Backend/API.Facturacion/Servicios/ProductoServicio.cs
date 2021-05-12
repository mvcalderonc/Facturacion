using API.Facturacion.DTO.Clases;
using API.Facturacion.DTO.Peticion;
using API.Facturacion.DTO.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Facturacion.Servicios
{
    public class ProductoServicio
    {
        public RespuestaProductoConsultar Listar()
        {
            var resultado = new RespuestaProductoConsultar();
            try
            {
                resultado.ListaProductos = consultarProductos(null, null, DateTime.Now, 1000);

                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            catch (Exception)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = "Ocurrio un error al consultar los productos.";
            }
            return resultado;
        }

        public RespuestaProductoConsultar Consultar(string codigo, string nombre)
        {
            var resultado = new RespuestaProductoConsultar();
            try
            {
                resultado.ListaProductos = consultarProductos(codigo, nombre, DateTime.Now, 10);
                
                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            catch (Exception)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = "Ocurrio un error al consultar los productos.";
            }
            return resultado;
        }


        public RespuestaProductoConsultar Obtener(int proId)
        {
            var resultado = new RespuestaProductoConsultar();
            try
            {
                var dataContext = new DAL.Facturacion.Models.FacturacionContext();
                var producto = dataContext.Producto.FirstOrDefault(pro => pro.Id == proId);

                if (producto != null)
                {
                    DateTime fechaActual = DateTime.Now;
                    var precioProducto = dataContext.PreciosFechas.FirstOrDefault(p => p.proId == proId &&
                                            p.FechaInicio <= fechaActual &&
                                            (p.FechaFinal == null || fechaActual < p.FechaFinal));

                    if (precioProducto != null)
                    {
                        resultado.ListaProductos = new List<Producto>();
                        resultado.ListaProductos.Add(new Producto
                        {
                            Id = producto.Id,
                            Codigo = producto.Codigo,
                            Nombre = producto.Nombre,
                            CantidadInventario = producto.CantidadInventario,
                            PrecioUnitario = precioProducto.PrecioUnitario
                        });

                        resultado.Estado = EstadoPeticion.OK;
                        resultado.Mensaje = "";
                    }
                    else
                    {
                        resultado.Estado = EstadoPeticion.ERROR;
                        resultado.Mensaje = "No se encontró un precio para el producto indicado.";
                    }
                }
                else
                {
                    resultado.Estado = EstadoPeticion.ERROR;
                    resultado.Mensaje = "No se encontró el producto indicado.";
                }
            }
            catch (Exception)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = "Ocurrio un error al consultar el producto.";
            }
            return resultado;
        }


        private List<Producto> consultarProductos(string codigo, string nombre, DateTime fecha, int numeroDatos)
        {
            codigo = (codigo ?? "").Trim();
            nombre = (nombre ?? "").Trim();

            var dataContext = new DAL.Facturacion.Models.FacturacionContext();
            var listaProductos = dataContext.Producto.Where(p => p.Activo == true).AsQueryable();

            if (!string.IsNullOrEmpty(codigo))
            {
                listaProductos = listaProductos.Where(pro => pro.Codigo.Contains(codigo));
            }

            if (!string.IsNullOrEmpty(codigo))
            {
                listaProductos = listaProductos.Where(pro => pro.Nombre.Contains(nombre));
            }


            var listaProductosPrecios = from prod in listaProductos
                                        join prfe in dataContext.PreciosFechas
                                            on prod.Id equals prfe.proId
                                        where prfe.FechaInicio <= fecha
                                        && ( prfe.FechaFinal == null || fecha < prfe.FechaFinal)
                                        select new Producto { 
                                            Id = prod.Id,
                                            Codigo = prod.Codigo,
                                            Nombre = prod.Nombre,
                                            CantidadInventario = prod.CantidadInventario,
                                            PrecioUnitario = prfe.PrecioUnitario
                                        };

            return listaProductosPrecios.OrderBy(p=>p.Codigo).Take(numeroDatos).ToList();
        }
    }
}
