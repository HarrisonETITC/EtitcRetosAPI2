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
    public class RetosController : ControllerBase
    {
        private readonly EtitcRetosContext _context;

        public RetosController(EtitcRetosContext context)
        {
            _context = context;
        }

        // GET: api/Retoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RetoMV>>> GetRetos()
        {
            var retos = await _context.Retos.ToListAsync();
            var intentos = await _context.Intentos.ToListAsync();
            var solicitudes = await _context.SolicitudRetos.ToListAsync();
            var docentes = await _context.Docentes.ToListAsync();
            var personas = await _context.Personas.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();

            var query = from ret in retos
                        join sol1 in solicitudes on ret.SolicitudId equals sol1.IdSolicitudReto into SolReto
                        from solret in SolReto.DefaultIfEmpty()
                        join doc in docentes on solret?.SolicitadoPor equals doc.IdDocente into SolDoc
                        from soldoc in SolDoc.DefaultIfEmpty()
                        join per in personas on soldoc?.PersonaId equals per.IdPersona into DocPer
                        from docper in DocPer.DefaultIfEmpty()
                        join emp in empresas on solret?.SolicitudExterna equals emp.IdEmpresa into SolEmp
                        from solemp in SolEmp.DefaultIfEmpty()

                        select new RetoMV
                        {
                            IdReto = ret.IdReto,
                            Titulo = ret.Titulo,
                            Descripcion = ret.Descripcion,
                            Creacion = ret.FechaCreacion,
                            RangoSemestral = ret.RangoSemestral,
                            Intentos = ret.MaxIntentos,
                            Estado = ret.Estado,
                            Privacidad = ret.Privacidad,
                            Realizado = intentos.Where(ito => ito.RetoId == ret.IdReto).Count(),
                            CreadoPor = docper?.Nombre?? solemp?.Nombre?? "ADMIN"
                        };
            return query.ToList();
        }

        [HttpGet("docentes/{docenteid}")]
        public async Task<ActionResult<IEnumerable<RetoMV>>> GetRetosDocentes(int docenteid)
        {
            var retos = await _context.Retos.ToListAsync();
            var intentos = await _context.Intentos.ToListAsync();
            var solicitudes = await _context.SolicitudRetos.Where(sol => sol.SolicitadoPor != null && sol.SolicitadoPor == docenteid).ToListAsync();
            var docentes = await _context.Docentes.ToListAsync();
            var personas = await _context.Personas.ToListAsync();

            var query = from ret in retos
                        join sol1 in solicitudes on ret.SolicitudId equals sol1.IdSolicitudReto
                        join doc in docentes on sol1.SolicitadoPor equals doc.IdDocente into SolDoc
                        from soldoc in SolDoc.DefaultIfEmpty()
                        join per in personas on soldoc?.PersonaId equals per.IdPersona into DocPer
                        from docper in DocPer.DefaultIfEmpty()

                        select new RetoMV
                        {
                            IdReto = ret.IdReto,
                            Titulo = ret.Titulo,
                            Descripcion = ret.Descripcion,
                            Creacion = ret.FechaCreacion,
                            RangoSemestral = ret.RangoSemestral,
                            Intentos = ret.MaxIntentos,
                            Estado = ret.Estado,
                            Privacidad = ret.Privacidad,
                            Realizado = intentos.Where(ito => ito.RetoId == ret.IdReto).Count(),
                            CreadoPor = docper?.Nombre?? "ADMIN"
                        };
            return query.ToList();
        }

        [HttpGet("empresas/{empresaid}")]
        public async Task<ActionResult<IEnumerable<RetoMV>>> GetRetosEmpresa(int empresaid)
        {
            var retos = await _context.Retos.ToListAsync();
            var intentos = await _context.Intentos.ToListAsync();
            var solicitudes = await _context.SolicitudRetos.Where(sol => sol.SolicitudExterna != null && sol.SolicitudExterna == empresaid).ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();

            var query = from ret in retos
                        join sol1 in solicitudes on ret.SolicitudId equals sol1.IdSolicitudReto
                        join emp1 in empresas on sol1.SolicitudExterna equals emp1.IdEmpresa into SolEmp
                        from solemp in SolEmp.DefaultIfEmpty()

                        select new RetoMV
                        {
                            IdReto = ret.IdReto,
                            Titulo = ret.Titulo,
                            Descripcion = ret.Descripcion,
                            Creacion = ret.FechaCreacion,
                            RangoSemestral = ret.RangoSemestral,
                            Intentos = ret.MaxIntentos,
                            Estado = ret.Estado,
                            Privacidad = ret.Privacidad,
                            Realizado = intentos.Where(ito => ito.RetoId == ret.IdReto).Count(),
                            CreadoPor = solemp?.Nombre?? "ADMIN"
                        };
            return query.ToList();
        }

        [HttpGet("estudiantes")]
        public async Task<ActionResult<IEnumerable<RetoMV>>> GetRetosEstudiantes()
        {
            var retos = await _context.Retos.Where(ret => ret.Estado == "A").ToListAsync();
            var intentos = await _context.Intentos.ToListAsync();
            var solicitudes = await _context.SolicitudRetos.ToListAsync();
            var docentes = await _context.Docentes.ToListAsync();
            var personas = await _context.Personas.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();

            var query = from ret in retos
                        join sol1 in solicitudes on ret.SolicitudId equals sol1.IdSolicitudReto into SolReto
                        from solret in SolReto.DefaultIfEmpty()
                        join doc in docentes on solret?.SolicitadoPor equals doc.IdDocente into SolDoc
                        from soldoc in SolDoc.DefaultIfEmpty()
                        join per in personas on soldoc?.PersonaId equals per.IdPersona into DocPer
                        from docper in DocPer.DefaultIfEmpty()
                        join emp in empresas on solret?.SolicitudExterna equals emp.IdEmpresa into SolEmp
                        from solemp in SolEmp.DefaultIfEmpty()

                        select new RetoMV
                        {
                            IdReto = ret.IdReto,
                            Titulo = ret.Titulo,
                            Descripcion = ret.Descripcion,
                            Creacion = ret.FechaCreacion,
                            RangoSemestral = ret.RangoSemestral,
                            Intentos = ret.MaxIntentos,
                            Estado = ret.Estado,
                            Privacidad = ret.Privacidad,
                            Realizado = intentos.Where(ito => ito.RetoId == ret.IdReto).Count(),
                            CreadoPor = docper?.Nombre ?? solemp?.Nombre ?? "ADMIN"
                        };
            return query.ToList();
        }

        // GET: api/Retoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reto>> GetReto(int id)
        {
            var reto = await _context.Retos.FindAsync(id);

            if (reto == null)
            {
                return NotFound();
            }

            return reto;
        }

        [HttpGet("solicitud/{id}")]
        public async Task<ActionResult<Reto>> GetRetoSolicitud(int id)
        {
            var reto = await _context.Retos.Where(ret => ret.SolicitudId == id).FirstOrDefaultAsync();

            if (reto == null)
            {
                return NotFound();
            }

            return reto;
        }

        [HttpGet("disponibles/{idestudiante}")]
        public async Task<ActionResult<IEnumerable<Reto>>> GetRetosDisponibles(int idestudiante)
        {
            var intentos = await _context.Intentos.Where(intento => intento.RegistradoPor == idestudiante).ToListAsync();
            List<int?> realizados = new();
            intentos.ForEach((intento) => {realizados.Add(intento.RetoId); });

            var retos = await _context.Retos.Where(ret => !realizados.Contains(ret.IdReto) && ret.Estado == "A").ToListAsync();
            return retos;
        }

        // PUT: api/Retoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReto(int id, Reto reto)
        {
            if (id != reto.IdReto)
            {
                return BadRequest();
            }

            _context.Entry(reto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RetoExists(id))
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

        // POST: api/Retoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reto>> PostReto(Reto reto)
        {
            _context.Retos.Add(reto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReto", new { id = reto.IdReto }, reto);
        }

        // DELETE: api/Retoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReto(int id)
        {
            var reto = await _context.Retos.FindAsync(id);
            if (reto == null)
            {
                return NotFound();
            }

            _context.Retos.Remove(reto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RetoExists(int id)
        {
            return _context.Retos.Any(e => e.IdReto == id);
        }
    }
}
