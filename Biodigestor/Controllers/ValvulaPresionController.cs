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
    public class ValvulaPresionController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public ValvulaPresionController(BiodigestorContext context)
        {
            _context = context;
        }

        // POST valvulaPresion
        [HttpPost]
        public async Task<ActionResult<ValvulaPresion>> PostValvulaPresion(ValvulaPresion valvulaPresion)
        {
            _context.ValvulasPresion.Add(valvulaPresion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetValvulaPresion), new { id = valvulaPresion.IdValvulaPresion }, valvulaPresion);
        }

        // GET valvulaPresion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ValvulaPresion>>> GetValvulaPresion()
        {
            return await _context.ValvulasPresion.ToListAsync();
        }

        // GET valvulaPresion/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ValvulaPresion>> GetValvulaPresion(int id)
        {
            var valvulaPresion = await _context.ValvulasPresion.FindAsync(id);

            if (valvulaPresion == null)
            {
                return NotFound();
            }

            return valvulaPresion;
        }

        // GET valvulaPresion/fecha/{fecha}
        // GET valvulaPresion/fecha/{fecha}
[HttpGet("fecha/{fecha}")]
public async Task<ActionResult<IEnumerable<ValvulaPresion>>> GetValvulaPresionByFecha(DateTime fecha)
{
    return await _context.ValvulasPresion.Where(t => t.FechaHora.Date == fecha.Date).ToListAsync();
}

        // PUT valvulaPresion/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutValvulaPresion(int id, ValvulaPresion valvulaPresion)
        {
            if (id != valvulaPresion.IdValvulaPresion)
            {
                return BadRequest();
            }

            _context.Entry(valvulaPresion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValvulaPresionExists(id))
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

        // DELETE valvulaPresion/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ValvulaPresion>> DeleteValvulaPresion(int id)
        {
            var valvulaPresion = await _context.ValvulasPresion.FindAsync(id);
            if (valvulaPresion == null)
            {
                return NotFound();
            }

            _context.ValvulasPresion.Remove(valvulaPresion);
            await _context.SaveChangesAsync();

            return valvulaPresion;
        }

        private bool ValvulaPresionExists(int id)
        {
            return _context.ValvulasPresion.Any(e => e.IdValvulaPresion == id);
        }
    }
}