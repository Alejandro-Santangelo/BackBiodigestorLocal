using System.ComponentModel.DataAnnotations;

namespace Biodigestor.Models
{
    public class Acumulador
    {    [Key]
        public int IdAcumulador { get; set; }
        public required string Estado { get; set; }
        public int Capacidad { get; set; }
        public int InputGas { get; set; }
        public int OutputGas { get; set; }
        public double VolumenGas { get; internal set; }
    }
}