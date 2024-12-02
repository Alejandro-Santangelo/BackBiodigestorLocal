using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Biodigestor.Model
{
    public class Domicilio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumeroMedidor { get; set; }  // PK e Identity

        [Required]
        public required string Calle { get; set; }  // Obligatorio

        [Required]
        public required string Numero { get; set; }  // Obligatorio

        public string? Piso { get; set; }  // Puede ser null

        public string? Departamento { get; set; }  // Puede ser null

        // Clave foránea para Cliente
        public int NumeroCliente { get; set; }  // FK a Cliente

        [JsonIgnore]
        [ForeignKey("NumeroCliente")]
        public Cliente? Cliente { get; set; }

        // Propiedad de navegación a Factura
        [JsonIgnore]
        public ICollection<Factura>? Facturas { get; set; }
    }
}





