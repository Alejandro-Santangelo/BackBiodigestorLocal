using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Biodigestor.DTOs
{
    public class SensorPresionDto
    {
        [JsonIgnore]
        [Required]
        public int IdSensor { get; set; }

        [Required]
        public int IdBiodigestor { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        // [Required]
        // [Range(0, 200)] // Ajusta el rango según sea necesario para las lecturas de presión
        public double ValorLectura { get; set; }  // Lectura del sensor de presión
    }
}


