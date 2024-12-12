using Biodigestor.Data;
using Biodigestor.Model;
using Biodigestor.Models;
using M
    icrosoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize(Roles = "Administracion, Manager,tecnico,cliente")] // Restringe el acceso por roles
[ApiController]
[Route("api/[controller]")]
public class FacturaController : ControllerBase
{
    private readonly BiodigestorContext _context;

    public FacturaController(BiodigestorContext context)
    {
        _context = context;
    }

   // POST: api/Facturas
    [HttpPost]
    public async Task<ActionResult<Factura>> PostFactura(Factura factura)
    {
    var dniClaim = User.FindFirst("DNI")?.Value;
    if (!int.TryParse(dniClaim, out int dniAutenticado))
    {
        return Unauthorized("Usuario no autenticado.");
    }

    // Verifica si el cliente existe
    var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.DNI == dniAutenticado);
    if (cliente == null)
    {
        return NotFound("Cliente no encontrado.");
    }

    // Asigna el cliente autenticado a la factura
    factura.ClienteId = cliente.Id;

    _context.Facturas.Add(factura);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetFacturaByNumeroFactura), new { numeroFactura = factura.NumeroFactura }, factura);
}


    // GET: api/Facturas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Factura>>> GetFacturas()
    {
        // Devuelve todas las facturas con sus relaciones
        return await _context.Facturas
            .Include(f => f.Cliente)  // Incluye los datos del cliente relacionados
            .Include(f => f.Domicilio)  // Incluye los datos del domicilio relacionados
            .AsNoTracking()  // Mejora el rendimiento para consultas de solo lectura
            .ToListAsync();
    }

    // GET: api/Facturas/{numeroFactura}
    [HttpGet("{numeroFactura:int}")]
    public async Task<ActionResult<Factura>> GetFacturaByNumeroFactura(int numeroFactura)
    {
        try
        {
            // Busca la factura por su número
            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Domicilio)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura);

            if (factura == null)
            {
                return NotFound($"Factura con NumeroFactura {numeroFactura} no encontrada.");
            }

            return factura;
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // GET: api/Facturas/cliente/{dni}
    [HttpGet("cliente/{dni}")]
    public async Task<ActionResult<IEnumerable<Factura>>> GetFacturasByDni(int dni)
    {
        // Busca facturas asociadas a un cliente específico por DNI
        var facturas = await _context.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Domicilio)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(f => f.Cliente != null && f.Cliente.DNI == dni)
            .ToListAsync();

        if (facturas == null || facturas.Count == 0)
        {
            return NotFound($"No se encontraron facturas para el cliente con DNI {dni}.");
        }

        return facturas;
    }

    // GET: api/Facturas/domicilio/{numeroMedidor}
    [HttpGet("domicilio/{numeroMedidor}")]
    public async Task<ActionResult<IEnumerable<Factura>>> GetFacturasByNumeroMedidor(int numeroMedidor)
    {
        // Busca facturas asociadas a un domicilio específico por número de medidor
        var facturas = await _context.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Domicilio)
            .AsSplitQuery()
            .AsNoTracking()
            .Where(f => f.Domicilio != null && f.Domicilio.NumeroMedidor == numeroMedidor)
            .ToListAsync();

        if (facturas == null || facturas.Count == 0)
        {
            return NotFound($"No se encontraron facturas para el número de medidor {numeroMedidor}.");
        }

        return facturas;
    }

    // PUT: api/Facturas/{numeroFactura}
    [HttpPut("{numeroFactura:int}")]
    public async Task<IActionResult> UpdateFactura(int numeroFactura, [FromBody] Factura updatedFactura)
    {
        // Busca la factura que deseas actualizar
        var factura = await _context.Facturas.FindAsync(numeroFactura);

        if (factura == null)
        {
            return NotFound($"Factura con NumeroFactura {numeroFactura} no encontrada.");
        }

        // Actualiza los campos permitidos
        factura.FechaVencimiento = updatedFactura.FechaVencimiento;
        factura.ConsumoMensual = updatedFactura.ConsumoMensual;

        // Guarda los cambios
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Facturas/{numeroFactura}
    [HttpDelete("{numeroFactura}")]
    public async Task<IActionResult> DeleteFactura(int numeroFactura)
    {
        // Busca la factura para eliminar
        var factura = await _context.Facturas
            .Include(f => f.Cliente)
            .Include(f => f.Domicilio)
            .FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura);

        if (factura == null)
        {
            return NotFound($"Factura con NumeroFactura {numeroFactura} no encontrada.");
        }

        // Elimina la factura
        _context.Facturas.Remove(factura);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}



