using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Docente
    {
        public int IdDocente { get; set; }
        public string? Materias { get; set; }
        public int? PersonaId { get; set; }

        public virtual Persona? Persona { get; set; }
        public virtual ICollection<Intento>? Intentos { get; set; }
        public virtual ICollection<SolicitudReto>? SolicitudRetos { get; set; }
    }
}
