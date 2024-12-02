using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Biodigestor.Model;
using Microsoft.EntityFrameworkCore;

namespace Biodigestor.Models
{
public class Registro
{
    [Key]
    public int IdRegistro { get; set; }  // Primary Key (Identity)
    public int IdSensor { get; set; }
    public int IdBiodigestor { get; set; }
    public string? TipoSensor { get; set; }
    public DateTime FechaHora { get; set; }
    public double? Alerta { get; set; }  // Puede ser null si no es un registro de alerta
    public double? Alarma { get; set; }  // Puede ser null si no es un registro de alarma
    public double? Normal { get; set; }  // Puede ser null si no es un registro normal
}
}
