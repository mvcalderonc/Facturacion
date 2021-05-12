using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Facturacion.DTO.Clases
{
    public class Factura
    {
        public int Id { get; set; }
        public Cliente Cliente { get; set; }
        public string Numero { get; set; }
        public DateTime FechaExpedicion { get; set; }
        public List<FacturaDetalle> ListaFacturaDetalles { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
