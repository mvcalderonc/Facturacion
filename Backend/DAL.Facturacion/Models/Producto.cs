using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAL.Facturacion.Models
{
    public partial class Producto
    {
        public Producto()
        {
            ListaFacturaDetalles = new HashSet<FacturaDetalle>();
            ListaPreciosProducto = new HashSet<PrecioProducto>();
        }

        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool? Activo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int CantidadInventario { get; set; }
        public int CantidadReposicion { get; set; }

        public virtual ICollection<FacturaDetalle> ListaFacturaDetalles { get; set; }
        public virtual ICollection<PrecioProducto> ListaPreciosProducto { get; set; }
    }
}
