using System.ComponentModel.DataAnnotations;

namespace Biodigestor.Models
{
    public class Calentador
    {
        [Key]
        public int IdCalentador { get; set; }
        public DateTime FechaHora { get; set; }
        public TimeSpan HoraEncendidoCalentador { get; set; }
        public TimeSpan HoraApagadoCalentador { get; set; }
    }
}