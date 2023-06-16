using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class ModuloPermiso
    {
        public int IdModuloPermiso { get; set; }
        public int? RolId { get; set; }
        public string? Permisos { get; set; }
        public int? ModuloId { get; set; }

        public virtual Modulo? Modulo { get; set; }
        public virtual Rol? Rol { get; set; }
    }
}
