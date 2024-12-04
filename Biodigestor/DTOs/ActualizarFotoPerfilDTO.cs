using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Biodigestor.DTOs
{
    public class ActualizarFotoPerfilDTO
    {
        [Required(ErrorMessage = "La foto es requerida")]
        public IFormFile? Foto { get; set; }
    }
}
