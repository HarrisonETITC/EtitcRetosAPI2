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
    public class ModuloPermisosController : ControllerBase
    {
        private readonly EtitcRetosContext _context;

        public ModuloPermisosController(EtitcRetosContext context)
        {
            _context = context;
        }

        // GET: api/ModuloPermisoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuloPermisoMV>>> GetModuloPermisos()
        {
            var modulosper = await _context.ModuloPermisos.ToListAsync();
            var modulos = await _context.Modulos.ToListAsync();
            var roles = await _context.Rols.ToListAsync();

            var query = from mop in modulosper
                        join mod in modulos on mop.ModuloId equals mod.IdModulo
                        join rol in roles on mop.RolId equals rol.IdRol
                        select new ModuloPermisoMV
                        {
                            ID = mop.IdModuloPermiso,
                            Rol = rol.TipoUsuario,
                            Modulo = mod.Nombre,
                            Permisos = mop.Permisos
                        };

            return query.ToList();
        }

        // GET: api/ModuloPermisoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModuloPermiso>> GetModuloPermiso(int id)
        {
            var moduloPermiso = await _context.ModuloPermisos.FindAsync(id);

            if (moduloPermiso == null)
            {
                return NotFound();
            }

            return moduloPermiso;
        }

        // PUT: api/ModuloPermisoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModuloPermiso(int id, ModuloPermiso moduloPermiso)
        {
            if (id != moduloPermiso.IdModuloPermiso)
            {
                return BadRequest();
            }

            _context.Entry(moduloPermiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuloPermisoExists(id))
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

        // POST: api/ModuloPermisoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModuloPermiso>> PostModuloPermiso(ModuloPermiso moduloPermiso)
        {
            _context.ModuloPermisos.Add(moduloPermiso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModuloPermiso", new { id = moduloPermiso.IdModuloPermiso }, moduloPermiso);
        }

        // DELETE: api/ModuloPermisoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModuloPermiso(int id)
        {
            var moduloPermiso = await _context.ModuloPermisos.FindAsync(id);
            if (moduloPermiso == null)
            {
                return NotFound();
            }

            _context.ModuloPermisos.Remove(moduloPermiso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModuloPermisoExists(int id)
        {
            return _context.ModuloPermisos.Any(e => e.IdModuloPermiso == id);
        }
    }
}
