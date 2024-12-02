using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // Añadido para JsonIgnore

namespace Biodigestor.Model
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumeroCliente { get; set; }  // PK e Identity

        
        public required int DNI { get; set; }  // Obligatorio

       
        public required string Nombre { get; set; }  // Obligatorio

        
        public required string Apellido { get; set; }  // Obligatorio

        public string? Email { get; set; }  // Puede ser null

        [JsonIgnore]
        public ICollection<Factura>? Facturas { get; set; }
        [JsonIgnore]
        public ICollection<Domicilio>? Domicilios { get; set; }
        // Si quieres excluir un campo de la serialización JSON, descomenta y usa el atributo JsonIgnore
        // [JsonIgnore]
        // public string CampoSecreto { get; set; }  // Ejemplo de campo excluido en la serialización JSON
    }
}
