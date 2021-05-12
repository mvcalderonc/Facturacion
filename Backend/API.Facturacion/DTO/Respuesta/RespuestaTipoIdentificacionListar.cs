using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Facturacion.DTO.Clases;

namespace API.Facturacion.DTO.Respuesta
{
    public class RespuestaTipoIdentificacionListar : Respuesta
    {
        public List<TipoIdentificacion> ListaTiposIdentificacion { get; set; }
    }
}
