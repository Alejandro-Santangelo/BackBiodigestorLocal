using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Biodigestor.Models
{
    [Table("SensoresTemperatura")]
    public class SensorTemperatura
{
    [Key]
    public int IdSensor { get; set; }
    
    public int IdBiodigestor { get; set; } // Esta es la clave foránea
    [JsonIgnore]
    [ForeignKey("IdBiodigestor")] // Indica que esta propiedad es la FK
    public virtual  Biodigestor? BiodigestorEntity { get; set; } 

    public DateTime FechaHora { get; set; }
}

}

