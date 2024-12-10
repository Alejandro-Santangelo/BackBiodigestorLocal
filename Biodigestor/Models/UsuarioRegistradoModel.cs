using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biodigestor.Models
{
    public class UsuarioRegistradoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuarioRegistrado { get; set; }
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public int DNI { get; set; }
        
        [Required]
        public string Rol { get; set; } = string.Empty;

        public byte[]? FotoPerfil { get; set; }

        public string? TipoContenidoFoto { get; set; }
    }
}
