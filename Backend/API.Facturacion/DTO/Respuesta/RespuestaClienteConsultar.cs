using API.Facturacion.DTO.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Facturacion.DTO.Respuesta
{
    public class RespuestaClienteConsultar : Respuesta
    {
        public List<Cliente> ListaClientes { get; set; }
    }
}
