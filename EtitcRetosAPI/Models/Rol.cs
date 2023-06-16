using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Rol
    {
        public Rol()
        {
            ModuloPermisos = new HashSet<ModuloPermiso>();
            Personas = new HashSet<Persona>();
        }

        public int IdRol { get; set; }
        public string? TipoUsuario { get; set; }
        public DateTime? Registro { get; set; }

        public virtual ICollection<ModuloPermiso>? ModuloPermisos { get; set; }
        public virtual ICollection<Persona>? Personas { get; set; }
    }
}
