using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biodigestor.Model; // Asegúrate de que el namespace es correcto
using System.Threading.Tasks;
using Biodigestor.Data;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers
{

    [Authorize(Roles = " Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly Data.BiodigestorContext _context;

        public PersonalController(BiodigestorContext context)
        {
            _context = context;
        }

        // GET: api/Personal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personal>>> GetPersonal()
        {
            return await _context.Personal.ToListAsync();
        }

        // GET: api/Personal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Personal>> GetPersonal(int id)
        {
            var personal = await _context.Personal.FindAsync(id);

            if (personal == null)
            {
                return NotFound(new { message = "Personal no encontrado." });
            }

            return personal;
        }

        // POST: api/Personal
        [HttpPost]
        public async Task<ActionResult<Personal>> PostPersonal(Personal personal)
        {
            // Validación si ya existe el DNI
            var existeDNI = await _context.Personal.AnyAsync(p => p.DNI == personal.DNI);
            if (existeDNI)
            {
                return BadRequest(new { message = "El DNI ya está registrado." });
            }

            _context.Personal.Add(personal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersonal), new { id = personal.Legajo }, personal);
        }

        // PUT: api/Personal/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonal(int id, Personal personal)
        {
            if (id != personal.Legajo)
            {
                return BadRequest(new { message = "El ID del personal no coincide." });
            }

            _context.Entry(personal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalExists(id))
                {
                    return NotFound(new { message = "Personal no encontrado." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Personal/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonal(int id)
        {
            var personal = await _context.Personal.FindAsync(id);
            if (personal == null)
            {
                return NotFound(new { message = "Personal no encontrado." });
            }

            _context.Personal.Remove(personal);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Personal eliminado con éxito." });
        }

        private bool PersonalExists(int id)
        {
            return _context.Personal.Any(e => e.Legajo == id);
        }
    }
}
