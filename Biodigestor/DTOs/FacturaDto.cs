using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Biodigestor.DTOs
{
   public class FacturaDto
{
    [Required]
    public int NumeroFactura { get; set; }

    [Required]
    public DateOnly FechaEmision { get; set; }
    
    [JsonIgnore]
    [Required]
    public DateOnly FechaVencimiento { get; set; }

    [Required]
    public decimal ConsumoMensual { get; set; }

    [Required]
    public decimal ConsumoTotal { get; set; }

    public DateOnly ProximoVencimiento => FechaVencimiento.AddDays(10);
}
}

