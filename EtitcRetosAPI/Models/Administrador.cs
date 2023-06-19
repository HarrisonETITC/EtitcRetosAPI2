using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Administrador
    {
        public int IdAdministrador { get; set; }
        public string? Cargo { get; set; }
        public int? PersonaId { get; set; }

        public virtual Persona? Persona { get; set; }
        public virtual ICollection<SolicitudReto>? SolicitudRetos { get; set; }
    }
}
