using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Biodigestor.Models;
using Biodigestor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers
{
    [Authorize(Roles = "Tecnico, Manager")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ApiController]
    [Route("api/[controller]")]
    public class ValvulaAguaController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public ValvulaAguaController(BiodigestorContext context)
        {
            _context = context;
        }

        // POST valvulaAgua
        [HttpPost]
        public async Task<ActionResult<ValvulaAgua>> PostValvulaAgua(ValvulaAgua valvulaAgua)
        {
            _context.ValvulasAgua.Add(valvulaAgua);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetValvulaAgua), new { id = valvulaAgua.IdValvulaAgua }, valvulaAgua);
        }

        // GET valvulaAgua
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ValvulaAgua>>> GetValvulaAgua()
        {
            return await _context.ValvulasAgua.ToListAsync();
        }

        // GET valvulaAgua/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ValvulaAgua>> GetValvulaAgua(int id)
        {
            var valvulaAgua = await _context.ValvulasAgua.FindAsync(id);

            if (valvulaAgua == null)
            {
                return NotFound();
            }

            return valvulaAgua;
        }

        // PUT valvulaAgua/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutValvulaAgua(int id, ValvulaAgua valvulaAgua)
        {
            if (id != valvulaAgua.IdValvulaAgua)
            {
                return BadRequest();
            }

            _context.Entry(valvulaAgua).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValvulaAguaExists(id))
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

        // DELETE valvulaAgua/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ValvulaAgua>> DeleteValvulaAgua(int id)
        {
            var valvulaAgua = await _context.ValvulasAgua.FindAsync(id);
            if (valvulaAgua == null)
            {
                return NotFound();
            }

            _context.ValvulasAgua.Remove(valvulaAgua);
            await _context.SaveChangesAsync();

            return valvulaAgua;
        }

        private bool ValvulaAguaExists(int id)
        {
            return _context.ValvulasAgua.Any(e => e.IdValvulaAgua == id);
        }
    }
}