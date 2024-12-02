using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Biodigestor.Models
{
    public class UsuarioRegistradoModel
    {
        [Key]
        public int IdUsuarioRegistrado { get; set; }

        public required string Username { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required int DNI { get; set; }

        [JsonIgnore]
        public string? Rol { get; set; }
    }
}
