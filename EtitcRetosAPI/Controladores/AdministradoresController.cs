using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EtitcRetosAPI.Models;
using EtitcRetosAPI.ModelView;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace EtitcRetosAPI.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        private readonly EtitcRetosContext _context;

        public AdministradoresController(EtitcRetosContext context)
        {
            _context = context;
        }

        // GET: api/Administradors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdministradorMV>>> GetAdministradors()
        {
            var administradores = await _context.Administradors.ToListAsync();
            var personas = await _context.Personas.ToListAsync();

            var query = from adm in administradores
                        join per in personas on adm.PersonaId equals per.IdPersona
                        select new AdministradorMV
                        {
                            IdAdministrador = adm.IdAdministrador,
                            Nombre = per.Nombre,
                            Cargo = adm.Cargo,
                            Estado = per.Estado
                        };
            return query.ToList();
        }

        // GET: api/Administradors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Administrador>> GetAdministrador(int id)
        {
            var administrador = await _context.Administradors.FindAsync(id);

            if (administrador == null)
            {
                return NotFound();
            }

            return administrador;
        }

        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<Administrador>> GetAdministradorUsuario(int id)
        {
            var personas = await _context.Personas.ToListAsync();
            var persona = personas.Where(per => per.UsuarioId == id).First();
            var admin = await _context.Administradors.Where(adm => adm.PersonaId == persona.IdPersona).FirstOrDefaultAsync();

            if (admin == null)
            {
                return NotFound();
            }
            admin.Persona = null;
            admin.SolicitudRetos = null;
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 10,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase// Opcional: Ajusta el límite de profundidad según sea necesario
            };

            var json = JsonSerializer.Serialize(admin, options);

            return Content(json, "application/json");
        }

        // PUT: api/Administradors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdministrador(int id, Administrador administrador)
        {
            if (id != administrador.IdAdministrador)
            {
                return BadRequest();
            }

            _context.Entry(administrador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministradorExists(id))
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

        // POST: api/Administradors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Administrador>> PostAdministrador(Administrador administrador)
        {
            _context.Administradors.Add(administrador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdministrador", new { id = administrador.IdAdministrador }, administrador);
        }

        // DELETE: api/Administradors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdministrador(int id)
        {
            var administrador = await _context.Administradors.FindAsync(id);
            if (administrador == null)
            {
                return NotFound();
            }

            _context.Administradors.Remove(administrador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdministradorExists(int id)
        {
            return _context.Administradors.Any(e => e.IdAdministrador == id);
        }
    }
}
