using API.Facturacion.DTO;
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
    public class TipoIdentificacionController : ControllerBase
    {
        [HttpGet]
        public RespuestaTipoIdentificacionListar Get()
        {
            return new TipoIdentificacionServicio().Listar();
        }
    }
}
