using ArbNet.Models;
using ArbNet.Models.DTOs;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSec.Cryptography;

namespace ArbNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ArbNetDbContext _context;

        public UsersController(ArbNetDbContext context)
        {
            _context = context;
        }

        // REGISTRO
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto model)
        {
            // Verificar si email existe
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "El correo ya está registrado"
                });
            }

            // Crear usuario con contraseña encriptada
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                RegisterDate = DateTime.UtcNow,
                Status = "Active",
                Country = model.Country ?? "Venezuela",
                SubscriptionType = "Free"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "Cuenta creada exitosamente",
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email
            });
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                });
            }

            // Verificar contraseña
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Contraseña incorrecta"
                });
            }

            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "Login exitoso",
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email
            });
        }
    }
}
