using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EtitcRetosAPI.Models;
using EtitcRetosAPI.ModelView;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace EtitcRetosAPI.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly EtitcRetosContext _context;

        public UsuariosController(EtitcRetosContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioVM>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            var personas = await _context.Personas.ToListAsync();
            var roles = await _context.Rols.ToListAsync();

            var query = from ur in usuarios 
                        join pa in personas on ur.IdUsuario equals pa.UsuarioId into UsuarioPersona
                        from userp in UsuarioPersona.DefaultIfEmpty()
                        join rol in roles on userp?.RolId equals rol.IdRol into PersonaRol
                        from rolp in PersonaRol.DefaultIfEmpty()
                        select new UsuarioVM
            {
                IdUsuario = ur.IdUsuario,
                Correo = ur.Correo,
                Persona = userp?.Nombre?? null,
                Rol = rolp?.TipoUsuario?? null,
                Estado = ur.Estado,
                Foto = ur.Fotoperfil
            };

            return query.ToList();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpGet("correo/{correo}")]
        public async Task<ActionResult<Usuario>> GetCorreo(String correo)
        {
            if(_context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

            if (usuario == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 2,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase// Opcional: Ajusta el límite de profundidad según sea necesario
            };

            var json = JsonSerializer.Serialize(usuario, options);

            return Content(json, "application/json");
        }

        // GET: api/Usuarios/5
        [HttpGet("sinpersona")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosSinPersona()
        {
            var personas = await _context.Personas.Where(per => per.UsuarioId != null).ToListAsync();
            List<int?> llaves = new();
            personas.ForEach((persona) => { llaves.Add(persona.UsuarioId); });
            var usuario = await _context.Usuarios.Where(ur => !llaves.Contains(ur.IdUsuario)).ToListAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpGet("info/{id}")]
        public async Task<ActionResult<UsuarioVM>> GetInfoUsuario(int id)
        {
            var usuarios = await _context.Usuarios.Where(u => u.IdUsuario == id).ToListAsync();
            var personas = await _context.Personas.ToListAsync();
            var roles = await _context.Rols.ToListAsync();

            var query = from user in usuarios
                        join per in personas on user.IdUsuario equals per.UsuarioId
                        join rol in roles on per.RolId equals rol.IdRol
                        select new UsuarioVM
                        {
                            IdUsuario = user.IdUsuario,
                            Correo = user.Correo,
                            Persona = per.Nombre,
                            Estado = per.Estado,
                            Rol = rol.TipoUsuario,
                            Foto = user.Fotoperfil
                        };
            var usuario = query.ToList().FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 2,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase// Opcional: Ajusta el límite de profundidad según sea necesario
            };

            var json = JsonSerializer.Serialize(usuario, options);

            return Content(json, "application/json");
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.IdUsuario }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
