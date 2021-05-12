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
    public class ProductoController : ControllerBase
    {
        [HttpGet]
        public RespuestaProductoConsultar Listar()
        {
            return new ProductoServicio().Listar();
        }

       [HttpGet("{codigo}/{nombre}")]
        public RespuestaProductoConsultar Consultar(string? codigo, string? nombre)
        {
            return new ProductoServicio().Consultar(codigo, nombre);
        }

        [HttpGet("{id}")]
        public RespuestaProductoConsultar Obtener(int id)
        {
            return new ProductoServicio().Obtener(id);
        }
    }
}
