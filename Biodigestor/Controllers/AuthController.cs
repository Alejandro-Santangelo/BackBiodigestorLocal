using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biodigestor.Data;
using Biodigestor.Models;
using System.Threading.Tasks;
using Biodigestor.DTOs;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Biodigestor.Controllers
{
    
    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly BiodigestorContext _context;

        public AuthController(BiodigestorContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioRegistradoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si el DNI está en la tabla Clientes o Personal
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.DNI == dto.DNI && c.Email == dto.Email);
            var personal = await _context.Personal.FirstOrDefaultAsync(p => p.DNI == dto.DNI && p.Email == dto.Email);

            string rol;
            if (cliente != null)
            {
                rol = "Cliente";
            }
            else if (personal != null)
            {
                rol = personal.Rol; // Asignar el rol basado en la tabla Personal
            }
            else
            {
                return BadRequest(new { message = "DNI o Email no encontrados en Clientes o Personal" });
            }

            // Verificar si el username o email ya están registrados en UsuariosRegistrados
            var existingUser = await _context.UsuariosRegistrados
                .AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email);

            if (existingUser)
            {
                return BadRequest(new { message = "El username o email ya están en uso." });
            }

            // Crear el nuevo usuario a partir del DTO
            var nuevoUsuario = new UsuarioRegistradoModel
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password, // Guardar la contraseña tal como se recibe (se recomienda encriptarla)
                DNI = dto.DNI,
                Rol = rol // Asignar rol
            };

            _context.UsuariosRegistrados.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registro exitoso" });
        }

        [HttpPost("verificarDNI")]
        public IActionResult VerificarDNI([FromBody] VerificarDNIRequest request)
        {
            if (request == null || request.DNI <= 0)
            {
                return BadRequest(new { message = "DNI inválido." });
            }

            var dni = request.DNI;

            // Verifica si el DNI existe en la tabla Clientes
            var cliente = _context.Clientes.FirstOrDefault(c => c.DNI == dni);
            if (cliente != null)
            {
                return Ok(new { message = "Cliente encontrado. Proceda al registro." });
            }

            // Verifica si el DNI existe en la tabla Personal
            var personal = _context.Personal.FirstOrDefault(p => p.DNI == dni);
            if (personal != null)
            {
                return Ok(new { message = "Personal encontrado. Proceda al registro." });
            }

            return BadRequest(new { message = "DNI no encontrado." });
        }

        [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(new { success = false, message = "Datos inválidos." });
    }

    // Buscar al usuario en la tabla UsuariosRegistrados por Username
    var usuario = await _context.UsuariosRegistrados
        .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

    if (usuario == null || usuario.Password != loginDto.Password)
    {
        return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
    }

    // Crear las claims (información sobre el usuario)
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.Username),
        new Claim(ClaimTypes.Role, usuario.Rol!), // Asignar el rol
        new Claim("DNI", usuario.DNI.ToString()) // Convertir DNI a string para agregarlo como claim
    };

    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

    // Autenticar al usuario y crear la cookie
    await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);

    return Ok(new 
    {   success = true,
        message = "Inicio de sesión exitoso", 
        usuario = usuario.Username, 
        rol = usuario.Rol // Devolver el rol del usuario
    });
}


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return Ok(new { message = "Sesión cerrada con éxito" });
        }

        [Authorize]
        [HttpPut("actualizar-credenciales")]
        public async Task<IActionResult> ActualizarCredenciales([FromBody] ActualizarCredencialesDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obtener el username del usuario autenticado
            var usernameActual = User.Identity?.Name;
            if (string.IsNullOrEmpty(usernameActual))
            {
                return Unauthorized(new { mensaje = "Usuario no autenticado" });
            }

            // Buscar al usuario en la base de datos
            var usuario = await _context.UsuariosRegistrados
                .FirstOrDefaultAsync(u => u.Username == usernameActual);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            // Verificar la contraseña actual
            if (usuario.Password != dto.PasswordActual)
            {
                return BadRequest(new { mensaje = "La contraseña actual es incorrecta" });
            }

            // Verificar si el nuevo username ya está en uso (si es diferente al actual)
            if (dto.NuevoUsername != usernameActual)
            {
                var existeUsername = await _context.UsuariosRegistrados
                    .AnyAsync(u => u.Username == dto.NuevoUsername);
        
                if (existeUsername)
                {
                    return BadRequest(new { mensaje = "El nuevo nombre de usuario ya está en uso" });
                }
            }

            // Actualizar las credenciales
            usuario.Username = dto.NuevoUsername;
            usuario.Password = dto.NuevaPassword;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Credenciales actualizadas correctamente" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar las credenciales" });
            }
        }
    }
}
