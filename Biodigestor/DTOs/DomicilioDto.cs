using System.ComponentModel.DataAnnotations;

namespace Biodigestor.DTOs
{
    public class DomicilioDto
{
    [Required]
    public int NumeroMedidor { get; set; }

    [Required]
    public required string Calle { get; set; }

    [Required]
    public required string Numero { get; set; }

    public string? Piso { get; set; }

    public string? Departamento { get; set; }
}
}
