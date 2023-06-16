using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string? Correo { get; set; }
        public string? Contraseña { get; set; }
        public DateTime? Registro { get; set; }
        public string? Fotoperfil { get; set; }

        public virtual ICollection<Persona>? Personas { get; set; }
    }
}
