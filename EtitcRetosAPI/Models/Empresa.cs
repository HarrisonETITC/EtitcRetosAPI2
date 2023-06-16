using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            SolicitudRetos = new HashSet<SolicitudReto>();
        }

        public int IdEmpresa { get; set; }
        public string? Nombre { get; set; }
        public string? Sector { get; set; }
        public string? Nit { get; set; }

        public virtual ICollection<SolicitudReto> SolicitudRetos { get; set; }
    }
}
