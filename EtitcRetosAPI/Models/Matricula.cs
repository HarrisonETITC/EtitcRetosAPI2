using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Matricula
    {

        public int IdMatricula { get; set; }
        public string? Estado { get; set; }
        public DateTime? ActivaDesde { get; set; }
        public DateTime? Vencimiento { get; set; }
        public string? Codigo { get; set; }

        public virtual ICollection<Estudiante>? Estudiantes { get; set; }
    }
}
