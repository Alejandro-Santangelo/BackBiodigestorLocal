using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Biodigestor.DTOs
{
    public class SensorTemperaturaDto
    {
        [JsonIgnore]
        [Required]
        public int IdSensor { get; set; }

        [Required]
        public int IdBiodigestor { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        // [Required]
        // [Range(-50, 150)] // Ajusta el rango según sea necesario para las lecturas de temperatura
        public double ValorLectura { get; set; }  // Lectura del sensor de temperatura
    }
}



