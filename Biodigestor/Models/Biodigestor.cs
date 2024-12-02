using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Biodigestor.Models
{
    [Table("BiodigestorEntities")]
    public class Biodigestor
    {
        [Key]
        public int IdBiodigestor { get; set; }
        public required string Descripcion { get; set; }
        public required string Estado { get; set; }
         /*
         [JsonIgnore]
        public ICollection<SensorTemperatura>? SensoresTemperatura { get; set; }
        */
    }
}