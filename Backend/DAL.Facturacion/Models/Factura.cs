using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAL.Facturacion.Models
{
    public partial class Factura
    {
        public Factura()
        {
            ListaFacturaDetalles = new HashSet<FacturaDetalle>();
        }

        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool? Activo { get; set; }
        public int CliId { get; set; }
        public string Numero { get; set; }
        public DateTime FechaExpedicion { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<FacturaDetalle> ListaFacturaDetalles { get; set; }
    }
}
