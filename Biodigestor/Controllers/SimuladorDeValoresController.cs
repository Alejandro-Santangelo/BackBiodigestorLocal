using Biodigestor.DTOs;
using Microsoft.AspNetCore.Mvc;
using Biodigestor.Data;
using Biodigestor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers;
[Authorize(Roles = "Manager, Tecnico")] 

[ApiController]
[Route("api/[controller]")]
public class SimuladorDeValoresController : ControllerBase
{
    private readonly BiodigestorContext _context;

    public SimuladorDeValoresController(BiodigestorContext context)
    {
        _context = context;
    }

    // POST: api/SimuladorDeValores/Humedad
     
    [HttpPost("sensor-humedad/{idSensor}")]
        public async Task<IActionResult> PostSensorHumedad(int idSensor, [FromBody] SensorHumedadDto sensorHumedadDto)
        {
            // Verifica si el sensor existe
            var sensorHumedad = await _context.SensoresHumedad.FindAsync(idSensor);
            if (sensorHumedad == null)
            {
                return NotFound("Sensor de Humedad no encontrado");
            }
            if (sensorHumedadDto.ValorLectura < 0 || sensorHumedadDto.ValorLectura > 100)
    {
        return BadRequest("El valor ingresado está por fuera de los valores manejados por este sensor. Debe estar entre 0% y 100%.");
    }

            // Inicializamos los campos Alerta, Alarma, Normal como nulos
            double? alerta = null;
            double? alarma = null;
            double? normal = null;

            // Lógica para determinar si el valor es Alerta, Alarma o Normal
            if (sensorHumedadDto.ValorLectura > 60 || sensorHumedadDto.ValorLectura < 20)
            {
                alarma = sensorHumedadDto.ValorLectura;
            }
            else if (sensorHumedadDto.ValorLectura > 50 & sensorHumedadDto.ValorLectura <= 60 || sensorHumedadDto.ValorLectura >= 20 & sensorHumedadDto.ValorLectura <= 30  )
            {
                alerta = sensorHumedadDto.ValorLectura;
            }
            else
            {
                normal = sensorHumedadDto.ValorLectura;
            }

            // Crear el nuevo registro
            var registro = new Registro
            {
                IdSensor = idSensor,
                IdBiodigestor = sensorHumedadDto.IdBiodigestor,
                TipoSensor = "Humedad",
                FechaHora = sensorHumedadDto.FechaHora,
                Alerta = alerta,
                Alarma = alarma,
                Normal = normal
            };

            // Guardamos el registro en la base de datos
            _context.Registros.Add(registro);
            await _context.SaveChangesAsync();

            return Ok(registro);
        }


    // POST para el sensor de temperatura
    [HttpPost("sensor-temperatura/{idSensor}")]
public async Task<IActionResult> PostSensorTemperatura(int idSensor, [FromBody] SensorTemperaturaDto sensorTemperaturaDto)
{

    var sensorTemperatura = await _context.SensoresTemperatura.FindAsync(idSensor);
            if (sensorTemperatura == null)
            {
                return NotFound("Sensor de Temperatura no encontrado");
            }

      if (sensorTemperaturaDto.ValorLectura < 0 || sensorTemperaturaDto.ValorLectura > 60)
    {
        return BadRequest("El valor ingresado está por fuera de los valores manejados por este sensor. Debe estar entre 0°C y 60°C.");
    } 

    // Inicializamos los campos Alerta, Alarma, Normal como nulos
    double? alerta = null;
    double? alarma = null;
    double? normal = null;

    // Lógica para determinar si el valor es Alerta, Alarma o Normal
    if (sensorTemperaturaDto.ValorLectura > 45 || sensorTemperaturaDto.ValorLectura < 20 )
    {
        // Si el valor de lectura es mayor a 100, lo asignamos a la columna Alarma
        alarma = sensorTemperaturaDto.ValorLectura;
    }
    else if (sensorTemperaturaDto.ValorLectura <= 30 & sensorTemperaturaDto.ValorLectura >= 20 || sensorTemperaturaDto.ValorLectura >= 40 & sensorTemperaturaDto.ValorLectura <= 45)
    {
        // Si está entre 80 y 100, lo asignamos a la columna Alerta
        alerta = sensorTemperaturaDto.ValorLectura;
    }
    else
    {
        // Si es 80 o menor, lo asignamos a la columna Normal
        normal = sensorTemperaturaDto.ValorLectura;
    }

    // Crear el nuevo registro con los datos correspondientes
    var registro = new Registro
    {
        IdSensor = idSensor,                          // El IdSensor viene de la URL
        IdBiodigestor = sensorTemperaturaDto.IdBiodigestor,  // El IdBiodigestor viene del DTO
        TipoSensor = "Temperatura",                   // Especificamos el tipo de sensor como Temperatura
        FechaHora = sensorTemperaturaDto.FechaHora,   // La fecha/hora de la lectura
        Alerta = alerta,                              // Se carga el valor en la columna Alerta si corresponde
        Alarma = alarma,                              // Se carga el valor en la columna Alarma si corresponde
        Normal = normal                               // Se carga el valor en la columna Normal si corresponde
    };

    // Guardamos el registro en la base de datos
    _context.Registros.Add(registro);
    await _context.SaveChangesAsync();

    // Retornamos el registro creado como respuesta
    return Ok(registro);
}


    // POST para el sensor de presión
    [HttpPost("sensor-presion/{idSensor}")]
public async Task<IActionResult> PostSensorPresion(int idSensor, [FromBody] SensorPresionDto sensorPresionDto)
{

    var sensorPresion = await _context.SensoresPresion.FindAsync(idSensor);
            if (sensorPresion == null)
            {
                return NotFound("Sensor de Presion no encontrado");
            }

     if (sensorPresionDto.ValorLectura < 0 || sensorPresionDto.ValorLectura > 7)
    {
        // Si el valor está fuera del rango permitido, retornamos un mensaje de error
        return BadRequest($"El valor ingresado ({sensorPresionDto.ValorLectura}) está fuera de los valores manejados por este sensor (0 - 7 psi).");
    }
    // Inicializamos los campos Alerta, Alarma, Normal como nulos
    double? alerta = null;
    double? alarma = null;
    double? normal = null;

    // Lógica para determinar si el valor es Alerta, Alarma o Normal
    if (sensorPresionDto.ValorLectura >=  7 || sensorPresionDto.ValorLectura <= 0.20)
    {
        // Si el valor de lectura es mayor a 150, lo asignamos a la columna Alarma
        alarma = sensorPresionDto.ValorLectura;
    }
    else if (sensorPresionDto.ValorLectura > 2.8 & sensorPresionDto.ValorLectura  < 7 || sensorPresionDto.ValorLectura < 0.7 & sensorPresionDto.ValorLectura > 0.20)
    {
        // Si está entre 120 y 150, lo asignamos a la columna Alerta<
        alerta = sensorPresionDto.ValorLectura;
    }
    else
    {
        // Si es 120 o menor, lo asignamos a la columna Normal
        normal = sensorPresionDto.ValorLectura;
    }

    // Crear el nuevo registro con los datos correspondientes
    var registro = new Registro
    {
        IdSensor = idSensor,                        // El IdSensor viene de la URL
        IdBiodigestor = sensorPresionDto.IdBiodigestor,  // El IdBiodigestor viene del DTO
        TipoSensor = "Presion",                     // Especificamos el tipo de sensor como Presión
        FechaHora = sensorPresionDto.FechaHora,     // La fecha/hora de la lectura
        Alerta = alerta,                            // Se carga el valor en la columna Alerta si corresponde
        Alarma = alarma,                            // Se carga el valor en la columna Alarma si corresponde
        Normal = normal                             // Se carga el valor en la columna Normal si corresponde
    };

    // Guardamos el registro en la base de datos
    _context.Registros.Add(registro);
    await _context.SaveChangesAsync();

    // Retornamos el registro creado como respuesta
    return Ok(registro);
}

}