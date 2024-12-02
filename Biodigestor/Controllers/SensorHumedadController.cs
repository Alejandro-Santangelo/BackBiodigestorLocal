using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Biodigestor.Models;
using Biodigestor.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers
{
    [Authorize(Roles = "Tecnico , Manager")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ApiController]
    [Route("api/[controller]")]
    public class SensorHumedadController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public SensorHumedadController(BiodigestorContext context)
        {
            _context = context;
        }

        // POST humedades
        [HttpPost]
        public async Task<ActionResult<SensorHumedad>> PostHumedad([FromBody] SensorHumedad sensorHumedad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SensoresHumedad.Add(sensorHumedad);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetHumedad), new { id = sensorHumedad.IdSensor }, sensorHumedad);
        }

        // GET humedades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorHumedad>>> GetHumedades()
        {
            return await _context.SensoresHumedad.ToListAsync();
        }

        // GET humedades/{fecha}
        [HttpGet("fecha/{fecha}")]
        public async Task<ActionResult<IEnumerable<SensorHumedad>>> GetHumedadesByFecha(DateTime fecha)
        {
            return await _context.SensoresHumedad
                .Where(t => t.FechaHora.Date == fecha.Date)
                .ToListAsync();
        }

        // GET humedad/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorHumedad>> GetHumedad(int id)
        {
            var sensorHumedad = await _context.SensoresHumedad.FindAsync(id);

            if (sensorHumedad == null)
            {
                return NotFound();
            }

            return sensorHumedad;
        }

        // PUT humedad/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHumedad(int id, SensorHumedad sensorHumedad)
        {
            if (id != sensorHumedad.IdSensor)
            {
                return BadRequest();
            }

            _context.Entry(sensorHumedad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorHumedadExists(id))
                {
                    return NotFound();
                }
                throw; // Re-throw the exception
            }

            return NoContent();
        }

        // DELETE humedad/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHumedad(int id)
        {
            var sensorHumedad = await _context.SensoresHumedad.FindAsync(id);
            if (sensorHumedad == null)
            {
                return NotFound();
            }

            _context.SensoresHumedad.Remove(sensorHumedad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SensorHumedadExists(int id)
        {
            return _context.SensoresHumedad.Any(e => e.IdSensor == id);
        }
    }
}


