﻿using API.Facturacion.DTO.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Facturacion.DTO.Respuesta
{
    public class RespuestaFacturaCrear : Respuesta
    {
        public Factura Factura { get; set; }
    }
}
