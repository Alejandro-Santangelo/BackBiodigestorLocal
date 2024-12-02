using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Biodigestor.Model
{
    public class Factura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumeroFactura { get; set; }  // PK e Identity

        [Required]
        public DateOnly FechaEmision { get; set; } 
        [JsonIgnore]
        [Required]
        public DateOnly FechaVencimiento { get; set; } 


        [Required]
        public decimal ConsumoMensual { get; set; }

        [Required]
        public decimal ConsumoTotal { get; set; }

        // Clave foránea para Cliente
        public int NumeroCliente { get; set; }

        [JsonIgnore]
        [ForeignKey("NumeroCliente")]
        public Cliente? Cliente { get; set; }

        // Clave foránea para Domicilio
        public int NumeroMedidor { get; set; }

        [JsonIgnore]
        [ForeignKey("NumeroMedidor")]
        public Domicilio? Domicilio { get; set; }

         public DateOnly ProximoVencimiento => FechaVencimiento.AddDays(10);
    }
}




