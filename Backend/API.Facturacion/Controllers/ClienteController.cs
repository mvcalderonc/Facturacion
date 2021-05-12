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
    public class ClienteController : ControllerBase
    {
        [HttpGet("{tidId}/{identificacion}")]
        public RespuestaClienteConsultar Consultar(int tidId, string identificacion)
        {
            return new ClienteServicio().Consultar(tidId, identificacion);
        }
    }
}
