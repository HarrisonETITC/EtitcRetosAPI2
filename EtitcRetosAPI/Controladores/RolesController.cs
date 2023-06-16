using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EtitcRetosAPI.Models;
using EtitcRetosAPI.ModelView;

namespace EtitcRetosAPI.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly EtitcRetosContext _context;

        public RolesController(EtitcRetosContext context)
        {
            _context = context;
        }

        // GET: api/Rols
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolMV>>> GetRols()
        {
            var roles = await _context.Rols.ToListAsync();
            var personas = await _context.Personas.ToListAsync();

            var query = from rol in roles
                        select new RolMV
                        {
                            IdRol = rol.IdRol,
                            TipoUsuario = rol.TipoUsuario,
                            Registro = rol.Registro
                        };
            return query.ToList();
        }

        // GET: api/Rols/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var rol = await _context.Rols.FindAsync(id);

            if (rol == null)
            {
                return NotFound();
            }

            return rol;
        }

        [HttpGet("usuario/{usuario}")]
        public async Task<ActionResult<RolMV>> GetRolUsuario(int usuario)
        {
            var personas = await _context.Personas.ToListAsync();
            var persona = personas.Find(per => per.UsuarioId == usuario);
            RolMV? rol = null;

            if (persona != null)
            {
                var roles = await _context.Rols.ToListAsync();
                var query = from r in roles
                            select new RolMV
                            {
                                IdRol = r.IdRol,
                                TipoUsuario = r.TipoUsuario,
                                Registro = r.Registro
                            };
                rol = query.Where(r => r.IdRol == persona.RolId).First();
            }

            if (rol == null)
            {
                return NotFound();
            }

            return rol;
        }

        // PUT: api/Rols/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, Rol rol)
        {
            if (id != rol.IdRol)
            {
                return BadRequest();
            }

            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
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

        // POST: api/Rols
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol(Rol rol)
        {
            _context.Rols.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRol", new { id = rol.IdRol }, rol);
        }

        // DELETE: api/Rols/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Rols.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            _context.Rols.Remove(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolExists(int id)
        {
            return _context.Rols.Any(e => e.IdRol == id);
        }
    }
}
