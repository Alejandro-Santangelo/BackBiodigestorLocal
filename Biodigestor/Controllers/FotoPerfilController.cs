using System;
using System.IO;
using System.Threading.Tasks;
using Biodigestor.DTOs;
using Biodigestor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Biodigestor.Data;

namespace Biodigestor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FotoPerfilController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public FotoPerfilController(BiodigestorContext context)
        {
            _context = context;
        }

        [HttpPost("subir")]
        public async Task<IActionResult> SubirFotoPerfil([FromForm] ActualizarFotoPerfilDTO modelo)
        {
            try
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    return Unauthorized("Usuario no autenticado");

                var usuario = await _context.UsuariosRegistrados
                    .FirstOrDefaultAsync(u => u.Username == username);
                    
                if (usuario == null)
                    return NotFound("Usuario no encontrado");

                if (modelo.Foto == null || modelo.Foto.Length == 0)
                    return BadRequest("No se ha proporcionado ninguna imagen");

                if (modelo.Foto.Length > 5242880) // 5MB max
                    return BadRequest("La imagen no debe superar los 5MB");

                var tiposPermitidos = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!Array.Exists(tiposPermitidos, x => x == modelo.Foto.ContentType))
                    return BadRequest("Tipo de archivo no permitido. Use JPG, PNG o GIF");

                using (var memoryStream = new MemoryStream())
                {
                    await modelo.Foto.CopyToAsync(memoryStream);
                    usuario.FotoPerfil = memoryStream.ToArray();
                    usuario.TipoContenidoFoto = modelo.Foto.ContentType;
                }

                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Foto de perfil actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerFotoPerfil()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized("Usuario no autenticado");

            var usuario = await _context.UsuariosRegistrados
                .FirstOrDefaultAsync(u => u.Username == username);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            if (usuario.FotoPerfil == null)
                return NotFound("El usuario no tiene foto de perfil");

            if (string.IsNullOrEmpty(usuario.TipoContenidoFoto))
                return BadRequest("Tipo de contenido de la foto no válido");

            return File(usuario.FotoPerfil, usuario.TipoContenidoFoto);
        }

        [HttpGet("obtener-por-usuario/{username}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ObtenerFotoPerfilPorUsername(string username)
        {
            try
            {
                // Log de entrada
                Console.WriteLine("==== Inicio de solicitud de foto ====");
                Console.WriteLine($"Recibida solicitud para obtener foto de usuario: '{username}'");
                Console.WriteLine($"Usuario actual: '{User.Identity?.Name}', Es autenticado: {User.Identity?.IsAuthenticated}");
                Console.WriteLine($"Roles del usuario: {string.Join(", ", User.Claims.Where(c => c.Type == "role").Select(c => c.Value))}");

                // Verificar autenticación y rol
                if (!User.Identity?.IsAuthenticated ?? true)
                {
                    Console.WriteLine("Usuario no autenticado");
                    return Unauthorized("Usuario no autenticado");
                }

                if (!User.IsInRole("Manager"))
                {
                    Console.WriteLine("Usuario no tiene rol Manager");
                    return Forbid("Se requiere rol Manager");
                }

                // Buscar usuario
                var usuario = await _context.UsuariosRegistrados
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

                if (usuario == null)
                {
                    Console.WriteLine($"Usuario no encontrado: '{username}'");
                    return NotFound($"Usuario '{username}' no encontrado");
                }

                Console.WriteLine($"Usuario encontrado: '{usuario.Username}'");
                Console.WriteLine($"Tiene foto: {usuario.FotoPerfil != null}");
                Console.WriteLine($"Tipo contenido: '{usuario.TipoContenidoFoto}'");

                if (usuario.FotoPerfil == null)
                {
                    Console.WriteLine($"Usuario {username} no tiene foto de perfil");
                    return NotFound($"El usuario '{username}' no tiene foto de perfil");
                }

                if (string.IsNullOrEmpty(usuario.TipoContenidoFoto))
                {
                    Console.WriteLine($"Tipo de contenido no válido para usuario {username}");
                    return BadRequest("Tipo de contenido de la foto no válido");
                }

                Console.WriteLine($"Enviando foto de perfil. Tamaño: {usuario.FotoPerfil.Length} bytes");
                Console.WriteLine("==== Fin de solicitud de foto ====");
                
                return File(usuario.FotoPerfil, usuario.TipoContenidoFoto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener foto: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }
    }
}
