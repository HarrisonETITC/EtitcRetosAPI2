using System;
using System.Collections.Generic;

namespace EtitcRetosAPI.Models
{
    public partial class Persona
    {
        public Persona()
        {
            Administradors = new HashSet<Administrador>();
            Docentes = new HashSet<Docente>();
            Estudiantes = new HashSet<Estudiante>();
        }

        public int IdPersona { get; set; }
        public string? Nombre { get; set; }
        public string? TipoIdentificacion { get; set; }
        public string? Identificacion { get; set; }
        public string? CodigoPais { get; set; }
        public string? Telefono { get; set; }
        public int? UsuarioId { get; set; }
        public int? RolId { get; set; }
        public string? Estado { get; set; }

        public virtual Rol? Rol { get; set; }
        public virtual Usuario? Usuario { get; set; }
        public virtual ICollection<Administrador> Administradors { get; set; }
        public virtual ICollection<Docente> Docentes { get; set; }
        public virtual ICollection<Estudiante> Estudiantes { get; set; }
    }
}
