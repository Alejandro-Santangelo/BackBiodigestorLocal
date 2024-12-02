using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Biodigestor.Models;
using Biodigestor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers
{   
    [Authorize(Roles = "Manager, Tecnico")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [ApiController]
    [Route("api/[controller]")]
    public class BiodigestoresController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public BiodigestoresController(BiodigestorContext context)
        {
            _context = context;
        }

        // GET: api/Biodigestores
       // GET: api/Biodigestores
      [HttpGet]
      public async Task<ActionResult<IEnumerable<Models.Biodigestor>>> GetBiodigestores()
      {
         return await _context.BiodigestorEntities.ToListAsync();
      }  

        // GET: api/Biodigestores/5
      [HttpGet("{id}")]
public async Task<ActionResult<Models.Biodigestor>> GetBiodigestor(int id)
{
    var biodigestor = await _context.BiodigestorEntities.FindAsync(id);

    if (biodigestor == null)
    {
        return NotFound();
    }

    return biodigestor;
}

        // PUT: api/Biodigestores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBiodigestor(int id, Models.Biodigestor biodigestor)
        {
            if (id != biodigestor.IdBiodigestor)
            {
                return BadRequest();
            }

            _context.Entry(biodigestor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BiodigestorExists(id))
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

        // POST: api/Biodigestores
        [HttpPost]
public async Task<ActionResult<Models.Biodigestor>> PostBiodigestor(Models.Biodigestor biodigestor)
{
    _context.BiodigestorEntities.Add(biodigestor);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetBiodigestor), new { id = biodigestor.IdBiodigestor }, biodigestor);
}

        // DELETE: api/Biodigestores/5
      [HttpDelete("{id}")]
public async Task<ActionResult<Models.Biodigestor>> DeleteBiodigestor(int id)
{
    var biodigestor = await _context.BiodigestorEntities.FindAsync(id);
    if (biodigestor == null)
    {
        return NotFound();
    }

    _context.BiodigestorEntities.Remove(biodigestor);
    await _context.SaveChangesAsync();

    return biodigestor;
}

        private bool BiodigestorExists(int id)
{
    return _context.BiodigestorEntities.Any(e => e.IdBiodigestor == id);
}
    }
}