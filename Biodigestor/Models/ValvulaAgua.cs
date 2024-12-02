using System.ComponentModel.DataAnnotations;

namespace Biodigestor.Models
{
    public class ValvulaAgua
    {
        [Key]
        public int IdValvulaAgua { get; set; }
        public DateTime FechaHora { get; set; }
        public TimeSpan VaHoraApertura { get; set; }
        public TimeSpan VaHoraCierre { get; set; }
    }
}