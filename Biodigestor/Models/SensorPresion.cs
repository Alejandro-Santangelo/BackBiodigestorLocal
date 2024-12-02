using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biodigestor.Models
{
    [Table("SensoresPresion")]
    public class SensorPresion
    {
        [Key]
        //public int IdRegistro { get; set; }
        public int IdSensor { get; set; }
        public int IdBiodigestor { get; set; }
        //public decimal ValorLectura { get; set; }  // Lectura del sensor de presión
        public DateTime FechaHora { get; set; }
        // public required string Estado { get; set; }
    }
}
