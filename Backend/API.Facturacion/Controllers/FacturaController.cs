using API.Facturacion.DTO.Peticion;
using API.Facturacion.DTO.Respuesta;
using API.Facturacion.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Facturacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        [HttpGet]
        public RespuestaFacturaConsultar Listar()
        {
            return new FacturaServicio().Listar();
        }

        [HttpGet("{identificacion}/{numero}")]
        public RespuestaFacturaConsultar Consultar(string identificacion, string numero)
        {
            return new FacturaServicio().Consultar(numero);
        }

        [HttpGet("{id}")]
        public RespuestaFacturaConsultar Obtener(int id)
        {
            return new FacturaServicio().Obtener(id);
        }

        [HttpPost]
        public RespuestaFacturaCrear Crear([FromBody] PeticionFacturaCrear datosFacturaCrear)
        {
            return new FacturaServicio().Crear(datosFacturaCrear);
        }

        [HttpPut("{id}")]
        public RespuestaFacturaCrear Actualizar(int id, [FromBody] PeticionFacturaCrear datosFacturaCrear)
        {
            return new FacturaServicio().Actualizar(id, datosFacturaCrear);
        }

        [HttpDelete("{id}")]
        public RespuestaFacturaEliminar Eliminar(int id)
        {
            return new FacturaServicio().Eliminar(id);
        }
    }
}
