using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biodigestor.Data;
using Biodigestor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosRegistradosController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public UsuariosRegistradosController(BiodigestorContext context)
        {
            _context = context;
        }

        // GET: api/UsuariosRegistrados
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsuarios()
        {
            return await _context.UsuariosRegistrados
                .Select(u => new
                {
                    u.IdUsuarioRegistrado,
                    u.Username,
                    u.Email,
                    u.DNI,
                    u.Rol,
                    TieneFoto = u.FotoPerfil != null
                })
                .ToListAsync();
        }

        // GET: api/UsuariosRegistrados/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<object>> GetUsuario(int id)
        {
            var usuario = await _context.UsuariosRegistrados
                .Select(u => new
                {
                    u.IdUsuarioRegistrado,
                    u.Username,
                    u.Email,
                    u.DNI,
                    u.Rol,
                    TieneFoto = u.FotoPerfil != null
                })
                .FirstOrDefaultAsync(u => u.IdUsuarioRegistrado == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // GET: api/UsuariosRegistrados/porDNI/{dni}
        [HttpGet("porDNI/{dni}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<object>> GetUsuarioPorDNI(int dni)
        {
            var usuario = await _context.UsuariosRegistrados
                .Select(u => new
                {
                    u.IdUsuarioRegistrado,
                    u.Username,
                    u.Email,
                    u.DNI,
                    u.Rol,
                    TieneFoto = u.FotoPerfil != null
                })
                .FirstOrDefaultAsync(u => u.DNI == dni);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // GET: api/UsuariosRegistrados/actual
        [HttpGet("actual")]
        [Authorize]
        public async Task<ActionResult<object>> GetUsuarioActual()
        {
            var usernameClaim = User.Identity?.Name;
            if (string.IsNullOrEmpty(usernameClaim))
            {
                return Unauthorized();
            }

            var usuario = await _context.UsuariosRegistrados
                .Select(u => new
                {
                    u.IdUsuarioRegistrado,
                    u.Username,
                    u.Email,
                    u.DNI,
                    u.Rol,
                    TieneFoto = u.FotoPerfil != null
                })
                .FirstOrDefaultAsync(u => u.Username == usernameClaim);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/UsuariosRegistrados/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioRegistradoModel usuario)
        {
            if (id != usuario.IdUsuarioRegistrado)
            {
                return BadRequest();
            }

            var usuarioExistente = await _context.UsuariosRegistrados.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos permitidos
            usuarioExistente.Username = usuario.Username;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Rol = usuario.Rol;
            // No actualizamos la contraseña ni la foto aquí por seguridad

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/UsuariosRegistrados/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.UsuariosRegistrados.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.UsuariosRegistrados.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.UsuariosRegistrados.Any(e => e.IdUsuarioRegistrado == id);
        }
    }
}
