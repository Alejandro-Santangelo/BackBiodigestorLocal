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
    public class SensorTemperaturaController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public SensorTemperaturaController(BiodigestorContext context)
        {
            _context = context;
        }

        // POST temperatura
        [HttpPost]
        public async Task<ActionResult<SensorTemperatura>> PostTemperatura([FromBody] SensorTemperatura sensorTemperatura)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SensoresTemperatura.Add(sensorTemperatura);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTemperatura), new { id = sensorTemperatura.IdSensor }, sensorTemperatura);
        }

        // GET temperatura
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorTemperatura>>> GetTemperaturas()
        {
            return await _context.SensoresTemperatura.ToListAsync();
        }

        // GET temperatura/{fecha}
        [HttpGet("fecha/{fecha}")]
        public async Task<ActionResult<IEnumerable<SensorTemperatura>>> GetTemperaturasByFecha(DateTime fecha)
        {
            return await _context.SensoresTemperatura
                .Where(t => t.FechaHora.Date == fecha.Date)
                .ToListAsync();
        }

        // GET temperatura/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorTemperatura>> GetTemperatura(int id)
        {
            var sensorTemperatura = await _context.SensoresTemperatura.FindAsync(id);

            if (sensorTemperatura == null)
            {
                return NotFound();
            }

            return sensorTemperatura;
        }

        // PUT temperatura/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTemperatura(int id, SensorTemperatura sensorTemperatura)
        {
            if (id != sensorTemperatura.IdSensor)
            {
                return BadRequest();
            }

            _context.Entry(sensorTemperatura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorTemperaturaExists(id))
                {
                    return NotFound();
                }
                throw; // Re-throw the exception
            }

            return NoContent();
        }

        // DELETE temperatura/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemperatura(int id)
        {
            var sensorTemperatura = await _context.SensoresTemperatura.FindAsync(id);
            if (sensorTemperatura == null)
            {
                return NotFound();
            }

            _context.SensoresTemperatura.Remove(sensorTemperatura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SensorTemperaturaExists(int id)
        {
            return _context.SensoresTemperatura.Any(e => e.IdSensor == id);
        }
    }
}


