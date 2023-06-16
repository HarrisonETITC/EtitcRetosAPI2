using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EtitcRetosAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using EtitcRetosAPI.ModelView;

namespace EtitcRetosAPI.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntentosController : ControllerBase
    {
        private readonly EtitcRetosContext _context;

        public IntentosController(EtitcRetosContext context)
        {
            _context = context;
        }

        // GET: api/Intentoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IntentoMV>>> GetIntentos()
        {
            var intentos = await _context.Intentos.ToListAsync();
            var docentes = await _context.Docentes.ToListAsync();
            var estudiantes = await _context.Estudiantes.ToListAsync();
            var retos = await _context.Retos.ToListAsync();
            var personas = await _context.Personas.ToListAsync();

            var query = from intent in intentos
                        join doc in docentes on intent.CalificadoPor equals doc.IdDocente into docGroup
                        from docente in docGroup.DefaultIfEmpty()
                        join per2 in personas on docente?.PersonaId equals per2.IdPersona into per2Group
                        from docentePersona in per2Group.DefaultIfEmpty()
                        join est in estudiantes on intent.RegistradoPor equals est.IdEstudiante into estGroup
                        from estudiante in estGroup.DefaultIfEmpty()
                        join per1 in personas on estudiante?.PersonaId equals per1.IdPersona into per1Group
                        from estudiantePersona in per1Group.DefaultIfEmpty()
                        join reto in retos on intent.RetoId equals reto.IdReto into retoGroup
                        from retoObj in retoGroup.DefaultIfEmpty()
                        select new IntentoMV
                        {
                            IdIntento = intent.IdIntento,
                            Registro = intent.Registro,
                            Estado = intent.Estado,
                            Adjuntos = intent.Adjuntos,
                            Retador = estudiantePersona?.Nombre ?? "Sin Retador",
                            Calificador = docentePersona?.Nombre ?? "Sin Calificador",
                            Nota = intent.Nota,
                            Observaciones = intent.Observaciones,
                            RetoTitulo = retoObj?.Titulo,
                        };

            return query.ToList();
        }

        // GET: api/Intentoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Intento>> GetIntento(int id)
        {
            var intento = await _context.Intentos.FindAsync(id);

            if (intento == null)
            {
                return NotFound();
            }

            return intento;

        }

        // PUT: api/Intentoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIntento(int id, Intento intento)
        {
            if (id != intento.IdIntento)
            {
                return BadRequest();
            }
            _context.Entry(intento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntentoExists(id))
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

        // POST: api/Intentoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Intento>> PostIntento(Intento intento)
        {
            _context.Intentos.Add(intento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIntento", new { id = intento.IdIntento }, intento);
        }

        // DELETE: api/Intentoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIntento(int id)
        {
            var intento = await _context.Intentos.FindAsync(id);
            if (intento == null)
            {
                return NotFound();
            }

            _context.Intentos.Remove(intento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IntentoExists(int id)
        {
            return _context.Intentos.Any(e => e.IdIntento == id);
        }
    }
}
