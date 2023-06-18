using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EtitcRetosAPI.Models
{
    public partial class EtitcRetosContext : DbContext
    {
        public EtitcRetosContext()
        {
        }

        public EtitcRetosContext(DbContextOptions<EtitcRetosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrador> Administradors { get; set; } = null!;
        public virtual DbSet<Docente> Docentes { get; set; } = null!;
        public virtual DbSet<Empresa> Empresas { get; set; } = null!;
        public virtual DbSet<Estudiante> Estudiantes { get; set; } = null!;
        public virtual DbSet<Intento> Intentos { get; set; } = null!;
        public virtual DbSet<Matricula> Matriculas { get; set; } = null!;
        public virtual DbSet<Modulo> Modulos { get; set; } = null!;
        public virtual DbSet<ModuloPermiso> ModuloPermisos { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;
        public virtual DbSet<Reto> Retos { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<SolicitudReto> SolicitudRetos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=HARRISON; Database=EtitcRetos; Trusted_Connection=True; TrustServerCertificate=Yes ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>(entity =>
            {
                entity.HasKey(e => e.IdAdministrador);

                entity.ToTable("Administrador");

                entity.Property(e => e.IdAdministrador).HasColumnName("id_administrador");

                entity.Property(e => e.Cargo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cargo");

                entity.Property(e => e.PersonaId).HasColumnName("personaId");

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.Administradors)
                    .HasForeignKey(d => d.PersonaId)
                    .HasConstraintName("FK_Administrador_Persona");
            });

            modelBuilder.Entity<Docente>(entity =>
            {
                entity.HasKey(e => e.IdDocente);

                entity.ToTable("Docente");

                entity.Property(e => e.IdDocente).HasColumnName("id_docente");

                entity.Property(e => e.Materias)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("materias");

                entity.Property(e => e.PersonaId).HasColumnName("personaId");

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.Docentes)
                    .HasForeignKey(d => d.PersonaId)
                    .HasConstraintName("FK_Docente_Persona");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.IdEmpresa);

                entity.ToTable("Empresa");

                entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");

                entity.Property(e => e.Nit)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NIT");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Sector)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sector");
            });

            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.HasKey(e => e.IdEstudiante);

                entity.ToTable("Estudiante");

                entity.Property(e => e.IdEstudiante).HasColumnName("id_estudiante");

                entity.Property(e => e.MatriculaId).HasColumnName("matriculaId");

                entity.Property(e => e.PersonaId).HasColumnName("personaId");

                entity.Property(e => e.Semestre).HasColumnName("semestre");

                entity.HasOne(d => d.Matricula)
                    .WithMany(p => p.Estudiantes)
                    .HasForeignKey(d => d.MatriculaId)
                    .HasConstraintName("FK_Estudiante_Matricula");

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.Estudiantes)
                    .HasForeignKey(d => d.PersonaId)
                    .HasConstraintName("FK_Estudiante_Persona");
            });

            modelBuilder.Entity<Intento>(entity =>
            {
                entity.HasKey(e => e.IdIntento);

                entity.ToTable("Intento");

                entity.Property(e => e.IdIntento).HasColumnName("id_intento");

                entity.Property(e => e.Adjuntos)
                    .HasColumnType("text")
                    .HasColumnName("adjuntos");

                entity.Property(e => e.CalificadoPor).HasColumnName("calificadoPor");

                entity.Property(e => e.Estado)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Nota)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("nota");

                entity.Property(e => e.Observaciones)
                    .HasColumnType("text")
                    .HasColumnName("observaciones");

                entity.Property(e => e.RegistradoPor).HasColumnName("registradoPor");

                entity.Property(e => e.Registro)
                    .HasColumnType("datetime")
                    .HasColumnName("registro");

                entity.Property(e => e.RetoId).HasColumnName("retoId");

                entity.HasOne(d => d.CalificadoPorNavigation)
                    .WithMany(p => p.Intentos)
                    .HasForeignKey(d => d.CalificadoPor)
                    .HasConstraintName("FK_Intento_Docente");

                entity.HasOne(d => d.RegistradoPorNavigation)
                    .WithMany(p => p.Intentos)
                    .HasForeignKey(d => d.RegistradoPor)
                    .HasConstraintName("FK_Intento_Estudiante");

                entity.HasOne(d => d.Reto)
                    .WithMany(p => p.Intentos)
                    .HasForeignKey(d => d.RetoId)
                    .HasConstraintName("FK_Intento_Reto");
            });

            modelBuilder.Entity<Matricula>(entity =>
            {
                entity.HasKey(e => e.IdMatricula);

                entity.ToTable("Matricula");

                entity.Property(e => e.IdMatricula).HasColumnName("id_matricula");

                entity.Property(e => e.ActivaDesde)
                    .HasColumnType("datetime")
                    .HasColumnName("activaDesde");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("codigo");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Vencimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("vencimiento");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.IdModulo);

                entity.ToTable("Modulo");

                entity.Property(e => e.IdModulo).HasColumnName("id_modulo");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .HasColumnName("descripcion");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<ModuloPermiso>(entity =>
            {
                entity.HasKey(e => e.IdModuloPermiso);

                entity.ToTable("ModuloPermiso");

                entity.Property(e => e.IdModuloPermiso).HasColumnName("id_moduloPermiso");

                entity.Property(e => e.ModuloId).HasColumnName("moduloId");

                entity.Property(e => e.Permisos)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("permisos");

                entity.Property(e => e.RolId).HasColumnName("rolId");

                entity.HasOne(d => d.Modulo)
                    .WithMany(p => p.ModuloPermisos)
                    .HasForeignKey(d => d.ModuloId)
                    .HasConstraintName("FK_ModuloPermiso_Modulo");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.ModuloPermisos)
                    .HasForeignKey(d => d.RolId)
                    .HasConstraintName("FK_ModuloPermiso_Rol");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.IdPersona);

                entity.ToTable("Persona");

                entity.Property(e => e.IdPersona).HasColumnName("id_persona");

                entity.Property(e => e.CodigoPais)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("codigoPais");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Identificacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("identificacion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.RolId).HasColumnName("rolId");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.Property(e => e.TipoIdentificacion)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("tipoIdentificacion");

                entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.RolId)
                    .HasConstraintName("FK_Persona_Rol");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK_Persona_Usuario");
            });

            modelBuilder.Entity<Reto>(entity =>
            {
                entity.HasKey(e => e.IdReto);

                entity.ToTable("Reto");

                entity.Property(e => e.IdReto).HasColumnName("id_reto");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .HasColumnName("descripcion");

                entity.Property(e => e.Estado)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion");

                entity.Property(e => e.MaxIntentos).HasColumnName("maxIntentos");

                entity.Property(e => e.Privacidad)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("privacidad");

                entity.Property(e => e.RangoSemestral)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("rangoSemestral");

                entity.Property(e => e.SolicitudId).HasColumnName("solicitudId");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("titulo");

                entity.HasOne(d => d.Solicitud)
                    .WithMany(p => p.Retos)
                    .HasForeignKey(d => d.SolicitudId)
                    .HasConstraintName("FK_Reto_solicitudReto");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol);

                entity.ToTable("Rol");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Registro)
                    .HasColumnType("datetime")
                    .HasColumnName("registro");

                entity.Property(e => e.TipoUsuario)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("tipoUsuario");
            });

            modelBuilder.Entity<SolicitudReto>(entity =>
            {
                entity.HasKey(e => e.IdSolicitudReto);

                entity.ToTable("solicitudReto");

                entity.Property(e => e.IdSolicitudReto).HasColumnName("id_solicitudReto");

                entity.Property(e => e.Estado)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.FechaSolicitado)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaSolicitado");

                entity.Property(e => e.Observacion)
                    .HasColumnType("text")
                    .HasColumnName("observacion");

                entity.Property(e => e.RevisadoPor).HasColumnName("revisadoPor");

                entity.Property(e => e.SolicitadoPor).HasColumnName("solicitadoPor");

                entity.Property(e => e.SolicitudExterna).HasColumnName("solicitudExterna");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tipo");

                entity.HasOne(d => d.RevisadoPorNavigation)
                    .WithMany(p => p.SolicitudRetos)
                    .HasForeignKey(d => d.RevisadoPor)
                    .HasConstraintName("FK_solicitudReto_Administrador");

                entity.HasOne(d => d.SolicitadoPorNavigation)
                    .WithMany(p => p.SolicitudRetos)
                    .HasForeignKey(d => d.SolicitadoPor)
                    .HasConstraintName("FK_solicitudReto_Docente");

                entity.HasOne(d => d.SolicitudExternaNavigation)
                    .WithMany(p => p.SolicitudRetos)
                    .HasForeignKey(d => d.SolicitudExterna)
                    .HasConstraintName("FK_solicitudReto_Empresa");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.ToTable("Usuario");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("contraseña");

                entity.Property(e => e.Correo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Fotoperfil)
                    .HasColumnType("text")
                    .HasColumnName("fotoperfil");

                entity.Property(e => e.Registro)
                    .HasColumnType("datetime")
                    .HasColumnName("registro");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
