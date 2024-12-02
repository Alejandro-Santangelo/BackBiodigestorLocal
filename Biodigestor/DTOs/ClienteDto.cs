using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biodigestor.DTOs
{
    public class ClienteDto
{
    [Required]
    public int DNI { get; set; }
    [Required]
    public required string Nombre { get; set; }
    [Required]
    public required string Apellido { get; set; }
    public string? Email { get; set; }
    public ICollection<DomicilioDto>? Domicilios { get; set; }
    public ICollection<FacturaDto>? Facturas { get; set; }
    public int NumeroCliente { get; set; } 
}
}

