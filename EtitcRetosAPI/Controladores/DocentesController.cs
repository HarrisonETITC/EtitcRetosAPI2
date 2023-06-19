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
    public class DocentesController : ControllerBase
    {
        private readonly EtitcRetosContext _context;

        public DocentesController(EtitcRetosContext context)
        {
            _context = context;
        }

        // GET: api/Docentes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocenteMV>>> GetDocentes()
        {
            var docentes = await _context.Docentes.ToListAsync();
            var personas = await _context.Personas.ToListAsync();

            var query = from doc in docentes
                        join per in personas on doc.PersonaId equals per.IdPersona
                        select new DocenteMV
                        {
                            IdDocente = doc.IdDocente,
                            Materias = doc.Materias,
                            Nombre = per.Nombre,
                            Estado = per.Estado
                        };

            return query.ToList();
        }

        // GET: api/Docentes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Docente>> GetDocente(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);

            if (docente == null)
            {
                return NotFound();
            }

            return docente;
        }

        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<Docente>> GetDocenteUsuario(int id)
        {
            var personas = await _context.Personas.ToListAsync();
            var persona = personas.Where(per => per.UsuarioId == id).First();
            var docente = await _context.Docentes.Where(doc => doc.PersonaId == persona.IdPersona).FirstOrDefaultAsync();

            if (docente == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 10,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(docente, options);

            return Content(json, "application/json");
        }

        [HttpGet("persona/{nombre}")]
        public async Task<ActionResult<Docente>> GetDocentePersona(string nombre)
        {
            var persona = await _context.Personas.Where(per => per.Nombre == nombre).FirstOrDefaultAsync();
            if (persona == null)
            {
                return NotFound();
            }

            var docente = await _context.Docentes.Where(doc => doc.PersonaId == persona.IdPersona).FirstOrDefaultAsync();

            if (docente == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 10,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase// Opcional: Ajusta el límite de profundidad según sea necesario
            };

            var json = JsonSerializer.Serialize(docente, options);

            return Content(json, "application/json");
        }

        // PUT: api/Docentes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocente(int id, Docente docente)
        {
            if (id != docente.IdDocente)
            {
                return BadRequest();
            }

            _context.Entry(docente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocenteExists(id))
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

        // POST: api/Docentes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Docente>> PostDocente(Docente docente)
        {
            _context.Docentes.Add(docente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocente", new { id = docente.IdDocente }, docente);
        }

        // DELETE: api/Docentes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocente(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null)
            {
                return NotFound();
            }

            _context.Docentes.Remove(docente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocenteExists(int id)
        {
            return _context.Docentes.Any(e => e.IdDocente == id);
        }
    }
}
