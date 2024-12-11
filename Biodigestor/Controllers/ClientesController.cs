using Biodigestor.Data;
using Biodigestor.DTOs;
using Biodigestor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize(Roles = "Administracion, Manager, Tecnico, Cliente")]
[Route("UsuarioAdministrador/[controller]")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly BiodigestorContext _context;

    public ClienteController(BiodigestorContext context)
    {
        _context = context;
    }

    // GET: api/Clientes
    [HttpGet]
    public async Task<ActionResult> GetClientes()
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var dniClaim = User.FindFirst("DNI")?.Value;

        // Si el rol es Cliente, devuelve solo los datos de ese cliente
        if (userRole == "Cliente" && int.TryParse(dniClaim, out int dniParsed))
        {
            // Devolver un único cliente
            var cliente = await GetClienteByDni(dniParsed);
            return Ok(cliente);
        }

        // Si el rol es Administrador, Manager o Tecnico, devuelve todos los clientes
        if (userRole == "Administracion" || userRole == "Manager" || userRole == "Tecnico")
        {
            // Devolver todos los clientes
            var clientes = await GetAllClientes();
            return Ok(clientes);
        }

        // Si no se cumple ninguna de las condiciones anteriores, se retorna un error
        return Unauthorized(new { message = "No tiene permisos para acceder a esta información." });
    }

    // Método para obtener todos los clientes
    private async Task<IEnumerable<ClienteDto>> GetAllClientes()
    {
        return await _context.Clientes
            .Select(c => new ClienteDto
            {
                NumeroCliente = c.NumeroCliente,
                DNI = c.DNI,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Email = c.Email
            })
            .ToListAsync();
    }

    // Método para obtener un cliente por su DNI (cuando el rol es Cliente)
    private async Task<ClienteDto> GetClienteByDni(int dniParsed)
    {
        var cliente = await _context.Clientes
            .Where(c => c.DNI == dniParsed)
            .Select(c => new ClienteDto
            {
                NumeroCliente = c.NumeroCliente,
                DNI = c.DNI,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Email = c.Email
            })
            .FirstOrDefaultAsync();

        if (cliente == null)
        {
            throw new Exception("Cliente no encontrado");
        }

        return cliente;
    }

    // GET: api/Clientes/dni
    [HttpGet("dni")]
    public async Task<ActionResult<ClienteDto>> GetClienteDatosTotales()
    {
        // Obtener el DNI del cliente autenticado desde las claims
        var dniClaim = User.FindFirst("DNI")?.Value;

        // Verificar que el claim DNI exista y sea válido
        if (dniClaim == null || !int.TryParse(dniClaim, out int dniAutenticado))
        {
            return BadRequest(new { message = "DNI no válido o no autenticado." });
        }

        // Buscar los datos del cliente autenticado
        var cliente = await _context.Clientes
            .Include(c => c.Domicilios)
            .Include(c => c.Facturas)
            .FirstOrDefaultAsync(c => c.DNI == dniAutenticado);

        // Verificar si el cliente existe
        if (cliente == null)
        {
            return NotFound(new { message = "Cliente no encontrado." });
        }

        // Mapear a ClienteDto
        var clienteDto = new ClienteDto
        {
            NumeroCliente = cliente.NumeroCliente,
            DNI = cliente.DNI,
            Nombre = cliente.Nombre,
            Apellido = cliente.Apellido,
            Email = cliente.Email,
            Domicilios = cliente.Domicilios?.Select(d => new DomicilioDto
            {
                NumeroMedidor = d.NumeroMedidor,
                Calle = d.Calle,
                Numero = d.Numero,
                Piso = d.Piso,
                Departamento = d.Departamento
            }).ToList(),
            Facturas = cliente.Facturas?.Select(f => new FacturaDto
            {
                NumeroFactura = f.NumeroFactura,
                FechaEmision = f.FechaEmision,
                FechaVencimiento = f.FechaVencimiento,
                ConsumoMensual = f.ConsumoMensual,
                ConsumoTotal = f.ConsumoTotal
            }).ToList()
        };

        return Ok(clienteDto);
    }

    // POST: api/Clientes
    [HttpPost]
    public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
    {
        // Validar el modelo
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Verificar si ya existe un cliente con el mismo DNI
        var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(c => c.DNI == cliente.DNI);
        if (clienteExistente != null)
        {
            // Retornar un conflicto si el DNI ya está registrado
            return Conflict(new { message = "Ya existe un cliente con ese DNI." });
        }

        // Agregar el nuevo cliente si no existe un cliente con el mismo DNI
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetClienteByDni), new { dni = cliente.DNI }, cliente);
    }

    // PUT: api/Clientes/{dni}
    [HttpPut("{dni}")]
    public async Task<IActionResult> PutCliente(int dni, Cliente cliente)
    {
        if (dni != cliente.DNI)
        {
            return BadRequest("El Numero de Cliente y D.N.I. no se pueden cambiar, debe ingresar los valores actuales.");
        }

        _context.Entry(cliente).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClienteExists(dni))
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

    // DELETE: api/Clientes/{dni}
    [HttpDelete("{dni}")]
    public async Task<IActionResult> DeleteCliente(int dni)
    {
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.DNI == dni);
        if (cliente == null)
        {
            return NotFound();
        }

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ClienteExists(int dni)
    {
        return _context.Clientes.Any(e => e.DNI == dni);
    }
}
