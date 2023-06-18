using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Modulo
    {
        public Modulo()
        {
            ModuloPermisos = new HashSet<ModuloPermiso>();
        }

        public int IdModulo { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }

        public virtual ICollection<ModuloPermiso>? ModuloPermisos { get; set; }
    }
}
