using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Biodigestor.Models;
using Biodigestor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers
{
    [Authorize(Roles = "Tecnico , Manager")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ApiController]
    [Route("api/[controller]")]
    public class AgitadorController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public AgitadorController(BiodigestorContext context)
        {
            _context = context;
        }

        // POST agitador
        [HttpPost]
        public async Task<ActionResult<Agitador>> PostAgitador(Agitador agitador)
        {
            _context.Agitadores.Add(agitador);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAgitador), new { id = agitador.IdAgitador }, agitador);
        }

        // GET agitadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agitador>>> GetAgitadores()
        {
            return await _context.Agitadores.ToListAsync();
        }

        // GET agitador/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Agitador>> GetAgitador(int id)
        {
            var agitador = await _context.Agitadores.FindAsync(id);

            if (agitador == null)
            {
                return NotFound();
            }

            return agitador;
        }

        // GET agitador/fecha/{fecha}
        [HttpGet("fecha/{fecha}")]
        public async Task<ActionResult<IEnumerable<Agitador>>> GetAgitadorByFecha(DateTime fecha)
        {
            return await _context.Agitadores.Where(a => a.FechaHora.Date == fecha.Date).ToListAsync();
        }
    }
}