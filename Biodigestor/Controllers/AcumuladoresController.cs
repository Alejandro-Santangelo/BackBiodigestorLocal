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
    public class AcumuladorController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public AcumuladorController(BiodigestorContext context)
        {
            _context = context;
        }

        // POST acumuladores
        [HttpPost]
        public async Task<ActionResult<Acumulador>> CreateAcumulador(Acumulador acumulador)
        {
            _context.Acumuladores.Add(acumulador);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAcumulador), new { id = acumulador.IdAcumulador }, acumulador);
        }

        // POST acumulador/inputGas
        [HttpPost("inputGas")]
        public async Task<ActionResult<Acumulador>> AddGas(int id, double volumenGas)
        {
            var acumulador = await _context.Acumuladores.FindAsync(id);
            if (acumulador == null)
            {
                return NotFound();
            }
            acumulador.VolumenGas += volumenGas;
            await _context.SaveChangesAsync();
            return Ok(acumulador);
        }

        // POST acumulador/outputGas
        [HttpPost("outputGas")]
        public async Task<ActionResult<Acumulador>> RemoveGas(int id, double volumenGas)
        {
            var acumulador = await _context.Acumuladores.FindAsync(id);
            if (acumulador == null)
            {
                return NotFound();
            }
            acumulador.VolumenGas -= volumenGas;
            await _context.SaveChangesAsync();
            return Ok(acumulador);
        }

        // GET acumuladores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Acumulador>>> GetAcumuladores()
        {
            return await _context.Acumuladores.ToListAsync();
        }

        // GET acumulador/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Acumulador>> GetAcumulador(int id)
        {
            var acumulador = await _context.Acumuladores.FindAsync(id);

            if (acumulador == null)
            {
                return NotFound();
            }

            return acumulador;
        }

        // PUT acumuladores
        [HttpPut("{id}")]
        public async Task<ActionResult<Acumulador>> UpdateAcumulador(int id, Acumulador acumulador)
        {
            if (id != acumulador.IdAcumulador)
            {
                return BadRequest();
            }
            _context.Entry(acumulador).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(acumulador);
        }

        // DELETE acumuladores
        [HttpDelete("{id}")]
        public async Task<ActionResult<Acumulador>> DeleteAcumulador(int id)
        {
            var acumulador = await _context.Acumuladores.FindAsync(id);

            if (acumulador == null)
            {
                return NotFound();
            }

            _context.Acumuladores.Remove(acumulador);
            await _context.SaveChangesAsync();
            return Ok(acumulador);
        }
    }
}