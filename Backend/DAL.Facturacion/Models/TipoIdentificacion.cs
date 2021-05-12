using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAL.Facturacion.Models
{
    public partial class TipoIdentificacion
    {
        public TipoIdentificacion()
        {
            ListaClientes = new HashSet<Cliente>();
        }

        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public bool? Activo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Cliente> ListaClientes { get; set; }
    }
}
