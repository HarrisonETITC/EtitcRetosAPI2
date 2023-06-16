using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Intento
    {
        public int IdIntento { get; set; }
        public DateTime? Registro { get; set; }
        public string? Estado { get; set; }
        public string? Adjuntos { get; set; }
        public int? RegistradoPor { get; set; }
        public int? CalificadoPor { get; set; }
        public decimal? Nota { get; set; }
        public string? Observaciones { get; set; }
        public int? RetoId { get; set; }

        public virtual Docente? CalificadoPorNavigation { get; set; }
        public virtual Estudiante? RegistradoPorNavigation { get; set; }
        public virtual Reto? Reto { get; set; }
    }
}
