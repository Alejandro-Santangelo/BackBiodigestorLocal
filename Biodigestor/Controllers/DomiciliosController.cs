using Biodigestor.Data;
using Biodigestor.Model;
using Biodigestor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


[Authorize(Roles = "Administracion , Manager")]
[ApiController]
[Route("USUARIO ADMINISTRADOR/[controller]")]
//[ApiExplorerSettings(GroupName = "Usuario Administrador")]
public class DomicilioController : ControllerBase
{
    private readonly BiodigestorContext _context;

    public DomicilioController(BiodigestorContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Domicilio>>> GetDomicilios()
    {
        var domicilios = await _context.Domicilios
            .Include(d => d.Cliente)
            .Include(d => d.Facturas)
            .ToListAsync();

        return domicilios;
    }

    [HttpGet("{numeroMedidor}")]
    public async Task<ActionResult<Domicilio>> GetDomicilioByNumeroMedidor(int numeroMedidor)
    {
        var domicilio = await _context.Domicilios
            .Include(d => d.Cliente)
            .Include(d => d.Facturas)
            .FirstOrDefaultAsync(d => d.NumeroMedidor == numeroMedidor);

        if (domicilio == null)
        {
            return NotFound();
        }

        return domicilio;
    }

    [HttpPost]
public async Task<ActionResult<Domicilio>> PostDomicilio(Domicilio domicilio)
{
    // Verificar si ya existe un domicilio con los mismos valores para Calle, Número, Piso y Departamento
    var domicilioExistente = await _context.Domicilios
        .FirstOrDefaultAsync(d => d.Calle == domicilio.Calle
                               && d.Numero == domicilio.Numero
                               && d.Piso == domicilio.Piso
                               && d.Departamento == domicilio.Departamento);

    // Si encuentra un domicilio con los mismos 4 valores, devolver mensaje de conflicto
    if (domicilioExistente != null)
    {
        return Conflict("El domicilio ya está registrado con Calle, Número, Piso y Departamento iguales.");
    }

    // Si no se encontró una coincidencia exacta, agregar el nuevo domicilio
    _context.Domicilios.Add(domicilio);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetDomicilioByNumeroMedidor), new { numeroMedidor = domicilio.NumeroMedidor }, domicilio);
}


    [HttpDelete("{numeroMedidor}")]
    public async Task<IActionResult> DeleteDomicilio(int numeroMedidor)
    {
        var domicilio = await _context.Domicilios.FindAsync(numeroMedidor);
        if (domicilio == null)
        {
            return NotFound();
        }

        _context.Domicilios.Remove(domicilio);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DomicilioExists(int numeroMedidor)
    {
        return _context.Domicilios.Any(d => d.NumeroMedidor == numeroMedidor);
    }
}

