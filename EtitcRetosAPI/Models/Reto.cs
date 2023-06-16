using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Reto
    {
        public Reto()
        {
            Intentos = new HashSet<Intento>();
        }

        public int IdReto { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? RangoSemestral { get; set; }
        public int? MaxIntentos { get; set; }
        public string? Estado { get; set; }
        public string? Privacidad { get; set; }
        public int? SolicitudId { get; set; }

        public virtual SolicitudReto? Solicitud { get; set; }
        public virtual ICollection<Intento>? Intentos { get; set; }
    }
}
