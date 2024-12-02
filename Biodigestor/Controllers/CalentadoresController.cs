using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Biodigestor.Models;
using Biodigestor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers
{   [Authorize(Roles = "Manager, Tecnico")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ApiController]
    [Route("api/[controller]")]
    public class CalentadorController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public CalentadorController(BiodigestorContext context)
        {
            _context = context;
        }

        // POST calentador
        [HttpPost]
        public async Task<ActionResult<Calentador>> PostCalentador(Calentador calentador)
        {
            _context.Calentadores.Add(calentador);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCalentador), new { id = calentador.IdCalentador }, calentador);
        }

        // GET calentadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Calentador>>> GetCalentadores()
        {
            return await _context.Calentadores.ToListAsync();
        }
        

        // GET calentador/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Calentador>> GetCalentador(int id)
        {
            var calentador = await _context.Calentadores.FindAsync(id);

            if (calentador == null)
            {
                return NotFound();
            }

            return calentador;
        }

        // GET calentador/fecha/{fecha}
        [HttpGet("fecha/{fecha}")]
        public async Task<ActionResult<IEnumerable<Calentador>>> GetCalentadorByFecha(DateTime fecha)
        {
            return await _context.Calentadores.Where(c => c.FechaHora.Date == fecha.Date).ToListAsync();
        }
    }
}