using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biodigestor.Data;
using Biodigestor.Models; // Asegúrate de importar el espacio de nombres correcto
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers
{
    [Authorize(Roles = "Manager, Tecnico")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrosController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public RegistrosController(BiodigestorContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "El contexto de la base de datos no ha sido inicializado.");
        }

        // GET: api/registros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registro>>> GetRegistros()
        {
            return await _context.Registros.ToListAsync();
        }

        // GET: api/registros/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Registro>> GetRegistro(int id)
        {
            var registro = await _context.Registros.FindAsync(id);

            if (registro == null)
            {
                return NotFound("No existe un registro con este Id");
            }

            return registro;
        }

        // GET: api/registros/fechas
        [HttpGet("fechas")]
        public async Task<ActionResult<IEnumerable<Registro>>> GetRegistrosByFecha(string fecha)
        {
            if (DateTime.TryParse(fecha, out var fechaParsed))
            {
                var registros = await _context.Registros
                    .Where(r => r.FechaHora.Date == fechaParsed.Date)
                    .ToListAsync();

                if (!registros.Any())
                {
                    return NotFound("No existen registros para esta fecha");
                }

                return Ok(registros);
            }
            else if (DateTime.TryParseExact(fecha, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out var fechaHoraParsed))
            {
                var registros = await _context.Registros
                    .Where(r => r.FechaHora == fechaHoraParsed)
                    .ToListAsync();

                if (!registros.Any())
                {
                    return NotFound("No existen registros para esta fecha ");
                }

                return Ok(registros);
            }
            else
            {
                return BadRequest("Formato de fecha no válido. Utiliza 'YYYY-MM-DD' o 'YYYY-MM-DD HH:mm'.");
            }
        }

        // GET: api/registros/biodigestor/{idBiodigestor}
        [HttpGet("biodigestor/{idBiodigestor}")]
        public async Task<ActionResult<IEnumerable<Registro>>> GetRegistrosByIdBiodigestor(int idBiodigestor)
        {
            var registros = await _context.Registros
                .Where(r => r.IdBiodigestor == idBiodigestor)
                .ToListAsync();

            if (!registros.Any())
            {
                return NotFound("No existe el Biodigestor"); // Devuelve 404 si no hay registros
            }

            return Ok(registros); // Devuelve los registros encontrados
        }

        // GET: api/registros/tiposensor/{tipoSensor}
        [HttpGet("tiposensor/{tipoSensor}")]
public async Task<ActionResult<IEnumerable<Registro>>> GetRegistrosByTipoSensor(string tipoSensor)
{
    // Asegúrate de que tipoSensor sea uno de los valores permitidos
    var tiposValidos = new List<string> { "Humedad", "Temperatura", "Presion" };
    if (!tiposValidos.Contains(tipoSensor))
    {
        return BadRequest("Tipo de sensor no válido. Los tipos válidos son: Humedad, Temperatura, Presion.");
    }

    var registros = await _context.Registros
        .Where(r => !string.IsNullOrEmpty(r.TipoSensor) && r.TipoSensor.ToLower() == tipoSensor.ToLower())
        .ToListAsync();

    if (!registros.Any())
    {
        return NotFound("El Biodigestor no tiene instalado este tipo de sensor."); // Devuelve 404 si no hay registros
    }

    return Ok(registros); // Devuelve los registros encontrados
}


   [HttpGet("alerta")]
public async Task<ActionResult<IEnumerable<Registro>>> GetRegistrosConAlerta()
{
    // Trae todos los registros donde Alerta no es null
    var registros = await _context.Registros
        .Where(r => r.Alerta.HasValue) // Filtra solo los registros donde Alerta no es null
        .ToListAsync();

    if (!registros.Any())
    {
        return new ObjectResult(new { message = "No existen registros de Alertas." }); // Devuelve 404 si no hay registros
    }

    return Ok(registros); // Devuelve los registros encontrados
}
[HttpGet("alarma")]
public async Task<ActionResult<IEnumerable<Registro>>> GetRegistrosConAlarma()
{
    // Trae todos los registros donde Alarma no es null
    var registros = await _context.Registros
        .Where(r => r.Alarma.HasValue) // Filtra solo los registros donde Alarma no es null
        .ToListAsync();

    if (!registros.Any())
    {
        return NotFound("No existen registros de Alarmas."); // Devuelve 404 si no hay registros
    }

    return Ok(registros); // Devuelve los registros encontrados
}
[HttpGet("normal")]
public async Task<ActionResult<IEnumerable<Registro>>> GetRegistrosConNormal()
{
    // Trae todos los registros donde Normal no es null
    var registros = await _context.Registros
        .Where(r => r.Normal.HasValue) // Filtra solo los registros donde Normal no es null
        .ToListAsync();

    if (!registros.Any())
    {
        return NotFound("No existen registros de Normales."); // Devuelve 404 si no hay registros
    }

    return Ok(registros); // Devuelve los registros encontrados
}

    }
    
}

