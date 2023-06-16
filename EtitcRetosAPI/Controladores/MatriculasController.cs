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
    public class MatriculasController : ControllerBase
    {
        private readonly EtitcRetosContext _context;

        public MatriculasController(EtitcRetosContext context)
        {
            _context = context;
        }

        // GET: api/Matriculas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatriculaMV>>> GetMatriculas()
        {
            var matriculas = await _context.Matriculas.ToListAsync();
            var estudiantes = await _context.Estudiantes.ToListAsync();
            var personas = await _context.Personas.ToListAsync();

            var query = from mat in matriculas 
                        join est in estudiantes on mat.IdMatricula equals est.MatriculaId into MatriculaEstudiante
                        from matest in MatriculaEstudiante.DefaultIfEmpty()
                        join per in personas on matest?.PersonaId equals per.IdPersona into PersonaEstudiante
                        from perest in PersonaEstudiante.DefaultIfEmpty()
                        select new MatriculaMV
                        {
                            IdMatricula = mat.IdMatricula,
                            Codigo = mat.Codigo,
                            Estado = mat.Estado,
                            ActivaDesde = mat.ActivaDesde,
                            ActivaHasta = mat.Vencimiento,
                            Estudiante = perest?.Nombre?? null
                        };
            return query.ToList();
        }

        // GET: api/Matriculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Matricula>> GetMatricula(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);

            if (matricula == null)
            {
                return NotFound();
            }

            return matricula;
        }

        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<Matricula>> GetMatricula(string codigo)
        {
            var matricula = await _context.Matriculas.Where(mat => mat.Codigo == codigo).FirstOrDefaultAsync();

            if (matricula == null)
            {
                return NotFound();
            }

            return matricula;
        }

        [HttpGet("libres")]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetMatriculasLibres()
        {
            var estudiantes = await _context.Estudiantes.ToListAsync();
            List<int?> ids = new();
            estudiantes.ForEach((est) => { ids.Add(est.MatriculaId); });

            var matriculas = await _context.Matriculas.Where(mat => !ids.Contains(mat.IdMatricula)).ToListAsync();

            if (matriculas == null)
            {
                return NotFound();
            }

            return matriculas;
        }

        // PUT: api/Matriculas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatricula(int id, Matricula matricula)
        {
            if (id != matricula.IdMatricula)
            {
                return BadRequest();
            }

            _context.Entry(matricula).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatriculaExists(id))
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

        // POST: api/Matriculas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Matricula>> PostMatricula(Matricula matricula)
        {
            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMatricula", new { id = matricula.IdMatricula }, matricula);
        }

        // DELETE: api/Matriculas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatricula(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula == null)
            {
                return NotFound();
            }

            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MatriculaExists(int id)
        {
            return _context.Matriculas.Any(e => e.IdMatricula == id);
        }
    }
}
