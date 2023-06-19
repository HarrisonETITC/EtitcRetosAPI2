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
    public class SolicitudRetosController : ControllerBase
    {
        private readonly EtitcRetosContext _context;

        public SolicitudRetosController(EtitcRetosContext context)
        {
            _context = context;
        }

        // GET: api/SolicitudRetoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudRetoMV>>> GetSolicitudRetos()
        {
            var solicitudes = await _context.SolicitudRetos.ToListAsync();
            var admins = await _context.Administradors.ToListAsync();
            var docentes = await _context.Docentes.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var personas = await _context.Personas.ToListAsync();

            var query = from sol in solicitudes 
                        join adm in admins on sol.RevisadoPor equals adm.IdAdministrador into SolicitudAdmin
                        from soladm in SolicitudAdmin.DefaultIfEmpty()
                        join doc in docentes on sol.SolicitadoPor equals doc.IdDocente into SolicitudDocente
                        from soldoc in SolicitudDocente.DefaultIfEmpty()
                        join emp in empresas on sol.SolicitudExterna equals emp.IdEmpresa into SolicitudEmpresa
                        from solemp in SolicitudEmpresa.DefaultIfEmpty()
                        join per in personas on soladm?.PersonaId equals per.IdPersona into SolicitudPersonaAdmin
                        from solper in SolicitudPersonaAdmin.DefaultIfEmpty()
                        join per1 in personas on soldoc?.PersonaId equals per1.IdPersona into SolicitudPersonaDocente
                        from docper in SolicitudPersonaDocente.DefaultIfEmpty()
                        select new SolicitudRetoMV
                        {
                            IdSolicitud = sol.IdSolicitudReto,
                            FechaSolicitado = sol.FechaSolicitado,
                            Reviso = solper?.Nombre?? null,
                            Solicito = docper?.Nombre?? null,
                            SolicitoEmpresa = solemp?.Nombre?? null,
                            Tipo = sol.Tipo,
                            Estado = sol.Estado,
                            Observacion = sol.Observacion

                        };
            return query.ToList();
        }

        // GET: api/SolicitudRetoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudReto>> GetSolicitudReto(int id)
        {
            var solicitudReto = await _context.SolicitudRetos.FindAsync(id);

            if (solicitudReto == null)
            {
                return NotFound();
            }

            return solicitudReto;
        }

        [HttpGet("empresa/{idempresa}")]
        public async Task<ActionResult<IEnumerable<SolicitudRetoMV>>> GetSolicitudRetoEmpresa(int idempresa)
        {
            var solicitudes = await _context.SolicitudRetos.Where(sol=> sol.SolicitudExterna != null && sol.SolicitudExterna == idempresa).ToListAsync();
            var admins = await _context.Administradors.ToListAsync();
            var docentes = await _context.Docentes.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var personas = await _context.Personas.ToListAsync();

            var query = from sol in solicitudes
                        join adm in admins on sol.RevisadoPor equals adm.IdAdministrador into SolicitudAdmin
                        from soladm in SolicitudAdmin.DefaultIfEmpty()
                        join doc in docentes on sol.SolicitadoPor equals doc.IdDocente into SolicitudDocente
                        from soldoc in SolicitudDocente.DefaultIfEmpty()
                        join emp in empresas on sol.SolicitudExterna equals emp.IdEmpresa into SolicitudEmpresa
                        from solemp in SolicitudEmpresa.DefaultIfEmpty()
                        join per in personas on soladm?.PersonaId equals per.IdPersona into SolicitudPersonaAdmin
                        from solper in SolicitudPersonaAdmin.DefaultIfEmpty()
                        join per1 in personas on soldoc?.PersonaId equals per1.IdPersona into SolicitudPersonaDocente
                        from docper in SolicitudPersonaDocente.DefaultIfEmpty()
                        select new SolicitudRetoMV
                        {
                            IdSolicitud = sol.IdSolicitudReto,
                            FechaSolicitado = sol.FechaSolicitado,
                            Reviso = solper?.Nombre ?? null,
                            Solicito = docper?.Nombre ?? null,
                            SolicitoEmpresa = solemp?.Nombre ?? null,
                            Tipo = sol.Tipo,
                            Estado = sol.Estado,
                            Observacion = sol.Observacion

                        };
            return query.ToList();
        }

        // PUT: api/SolicitudRetoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitudReto(int id, SolicitudReto solicitudReto)
        {
            if (id != solicitudReto.IdSolicitudReto)
            {
                return BadRequest();
            }

            _context.Entry(solicitudReto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudRetoExists(id))
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

        // POST: api/SolicitudRetoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SolicitudReto>> PostSolicitudReto(SolicitudReto solicitudReto)
        {
            _context.SolicitudRetos.Add(solicitudReto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitudReto", new { id = solicitudReto.IdSolicitudReto }, solicitudReto);
        }

        // DELETE: api/SolicitudRetoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitudReto(int id)
        {
            var solicitudReto = await _context.SolicitudRetos.FindAsync(id);
            if (solicitudReto == null)
            {
                return NotFound();
            }

            _context.SolicitudRetos.Remove(solicitudReto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SolicitudRetoExists(int id)
        {
            return _context.SolicitudRetos.Any(e => e.IdSolicitudReto == id);
        }
    }
}
