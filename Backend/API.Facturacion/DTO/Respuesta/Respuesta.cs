using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Facturacion.DTO.Respuesta
{
    public enum EstadoPeticion
    {
        OK = 1,
        ERROR = 2
    }
    public class Respuesta
    {
        public Respuesta()
        {
            Estado = EstadoPeticion.ERROR;
            Mensaje = "No se ha procesado la petición.";
        }
        public EstadoPeticion Estado { get; set; }
        public string Mensaje { get; set; }
    }
}
