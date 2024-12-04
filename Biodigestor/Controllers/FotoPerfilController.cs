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
    }
}
