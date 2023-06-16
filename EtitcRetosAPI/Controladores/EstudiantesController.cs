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
    public class EstudiantesController : ControllerBase
    {
        private readonly EtitcRetosContext _context;

        public EstudiantesController(EtitcRetosContext context)
        {
            _context = context;
        }

        // GET: api/Estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudianteMV>>> GetEstudiantes()
        {
            var estudiantes = await _context.Estudiantes.ToListAsync();
            var personas = await _context.Personas.ToListAsync();
            var matriculas = await _context.Matriculas.ToListAsync();

            var query = from est in estudiantes
                        join per in personas on est.PersonaId equals per.IdPersona
                        join mat in matriculas on est.MatriculaId equals mat.IdMatricula
                        select new EstudianteMV
                        {
                            IdEstudiante = est.IdEstudiante,
                            Semestre = est.Semestre,
                            Nombre = per.Nombre,
                            Matricula = mat.Codigo,
                            Estado = per.Estado
                        };

            return query.ToList();
        }

        // GET: api/Estudiantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
            {
                return NotFound();
            }

            return estudiante;
        }

        [HttpGet("usuario/{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudianteUsuario(int id)
        {
            var personas = await _context.Personas.ToListAsync();
            var persona = personas.Where(per => per.UsuarioId == id).First();
            var estudiante = await _context.Estudiantes.Where(est => est.PersonaId == persona.IdPersona).FirstOrDefaultAsync();

            if (estudiante == null)
            {
                return NotFound();
            }
            estudiante.Matricula = null;
            estudiante.Persona = null;
            estudiante.Intentos = null;
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 10,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase// Opcional: Ajusta el límite de profundidad según sea necesario
            };

            var json = JsonSerializer.Serialize(estudiante, options);

            return Content(json, "application/json");
        }

        // PUT: api/Estudiantes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiante(int id, Estudiante estudiante)
        {
            if (id != estudiante.IdEstudiante)
            {
                return BadRequest();
            }

            _context.Entry(estudiante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudianteExists(id))
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

        // POST: api/Estudiantes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante)
        {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstudiante", new { id = estudiante.IdEstudiante }, estudiante);
        }

        // DELETE: api/Estudiantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.IdEstudiante == id);
        }
    }
}
