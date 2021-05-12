using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Facturacion.DTO.Clases
{
    public class Cliente
    {
        public int Id { get; set; }
        public int TidId { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string CorreoElectronico { get; set; }
    }
}
