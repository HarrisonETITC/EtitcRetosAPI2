using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Estudiante
    {
        public int IdEstudiante { get; set; }
        public int? Semestre { get; set; }
        public int? PersonaId { get; set; }
        public int? MatriculaId { get; set; }

        public virtual Matricula? Matricula { get; set; }
        public virtual Persona? Persona { get; set; }
        public virtual ICollection<Intento>? Intentos { get; set; }
    }
}
