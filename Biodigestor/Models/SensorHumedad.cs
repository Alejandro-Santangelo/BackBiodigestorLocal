using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Biodigestor.Model;

namespace Biodigestor.Models
{
   [Table("SensoresHumedad")]
  public class SensorHumedad
{
    [Key]
    //public int IdRegistro { get; set; }
    public int IdSensor { get; set; }
    public int IdBiodigestor { get; set; }
    //public decimal ValorLectura { get; set; }
    public DateTime FechaHora { get; set; }
   // public required string Estado { get; set; }
}
}