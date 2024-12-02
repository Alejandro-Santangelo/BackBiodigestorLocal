namespace Biodigestor.Models
{
    public class RegistroUsuario
    {
        
        public required string NombreUsuario { get; set; }

        
        public required string Contraseña { get; set; }

        
        public required string ConfirmContraseña { get; set; }

        public required string Email { get; set; }
    }
}