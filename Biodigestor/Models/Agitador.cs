using System.ComponentModel.DataAnnotations;

namespace Biodigestor.Models
{
    public class Agitador
    {
        [Key]
        public int IdAgitador { get; set; }
        public DateTime FechaHora { get; set; }
        public TimeSpan HoraEncendidoAgitador { get; set; }
        public TimeSpan HoraApagadoAgitador { get; set; }
    }
}