using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Biodigestor.Data;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers
{
    [Authorize(Roles = " Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class VerificacionController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public VerificacionController(BiodigestorContext context)
        {
            _context = context;
        }

        // GET: api/verificacion/verificar-dni/{dni}
        [HttpGet("verificar-dni/{dni}")]
        public IActionResult VerificarDNI(int dni)
        {
            // Verificar si el DNI es válido
            if (dni <= 0)
            {
                return BadRequest(new { mensaje = "DNI inválido." });
            }

            // Verificar si el DNI existe en la tabla Clientes
            var cliente = _context.Clientes.FirstOrDefault(c => c.DNI == dni);
            // Verificar si el DNI existe en la tabla Personal
            var personal = _context.Personal.FirstOrDefault(p => p.DNI == dni);

            // Si el DNI se encuentra en Clientes o Personal
            if (cliente != null || personal != null)
            {
                return Ok(new { existe = true, mensaje = "Te podes Registrar" });
            }
            else
            {
                return Ok(new { existe = false, mensaje = " No estas autorizado para registrarte." });
            }
        }
    }

    // Modelo para la solicitud de verificación de DNI
    public class VerificacionRequest
    {
        public int DNI { get; set; }
    }
}
