using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // Añadido para JsonIgnore

namespace Biodigestor.Model
{
    public class Personal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Legajo { get; set; }  // PK e Identity

        public required string Rol { get; set; }  // Puede ser null

        
        public required int DNI { get; set; }  // Obligatorio

       
        public required string Nombre { get; set; }  // Obligatorio

        
        public required string Apellido { get; set; }  // Obligatorio

        public string? Email { get; set; }  // Puede ser null


      
    }
}