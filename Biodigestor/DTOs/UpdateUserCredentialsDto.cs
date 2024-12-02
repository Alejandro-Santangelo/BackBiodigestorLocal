using System.ComponentModel.DataAnnotations;

namespace Biodigestor.DTOs
{
    public class ActualizarCredencialesDto
    {
        [Required(ErrorMessage = "El nuevo nombre de usuario es requerido")]
        public required string NuevoUsername { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public required string NuevaPassword { get; set; }

        [Required(ErrorMessage = "La contraseña actual es requerida")]
        public required string PasswordActual { get; set; }
    }
}
