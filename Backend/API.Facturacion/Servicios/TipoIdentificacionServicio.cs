using API.Facturacion.DTO;
using API.Facturacion.DTO.Respuesta;
using API.Facturacion.DTO.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Facturacion.Servicios
{
    public class TipoIdentificacionServicio
    {
        public RespuestaTipoIdentificacionListar Listar()
        {
            var resultado = new RespuestaTipoIdentificacionListar();
            try
            {
                var dataContext = new DAL.Facturacion.Models.FacturacionContext();
                resultado.ListaTiposIdentificacion = dataContext.TipoIdentificacion.Select(tipo => new TipoIdentificacion { 
                    Id = tipo.Id,
                    Nombre = tipo.Nombre
                }).OrderBy(t=>t.Nombre).ToList();

                resultado.Estado = EstadoPeticion.OK;
                resultado.Mensaje = "";
            }
            catch (Exception)
            {
                resultado.Estado = EstadoPeticion.ERROR;
                resultado.Mensaje = "Ocurrio un error al consultar los tipos de identificación.";
            }
            return resultado;
        }
    }
}
