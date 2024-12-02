using System.ComponentModel.DataAnnotations;

namespace Biodigestor.Models
{
    public class ValvulaPresion
    {
        [Key]
        public int IdValvulaPresion { get; set; }
        public DateTime FechaHora { get; set; }
        public TimeSpan VpHoraApertura { get; set; }
        public TimeSpan VpHoraCierre { get; set; }
    }
}