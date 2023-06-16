using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class SolicitudReto
    {
        public SolicitudReto()
        {
            Retos = new HashSet<Reto>();
        }

        public int IdSolicitudReto { get; set; }
        public DateTime? FechaSolicitado { get; set; }
        public int? RevisadoPor { get; set; }
        public int? SolicitadoPor { get; set; }
        public string? Tipo { get; set; }
        public string? Estado { get; set; }
        public int? SolicitudExterna { get; set; }
        public string? Observacion { get; set; }

        public virtual Administrador? RevisadoPorNavigation { get; set; }
        public virtual Docente? SolicitadoPorNavigation { get; set; }
        public virtual Empresa? SolicitudExternaNavigation { get; set; }
        public virtual ICollection<Reto>? Retos { get; set; }
    }
}
