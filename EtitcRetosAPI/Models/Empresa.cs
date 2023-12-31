﻿using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Empresa
    {
        public int IdEmpresa { get; set; }
        public string? Nombre { get; set; }
        public string? Sector { get; set; }
        public string? Nit { get; set; }
        public string? Estado { get; set; }

        public virtual ICollection<SolicitudReto>? SolicitudRetos { get; set; }
    }
}
