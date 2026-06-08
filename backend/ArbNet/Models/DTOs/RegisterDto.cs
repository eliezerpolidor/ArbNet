using System.ComponentModel.DataAnnotations;

namespace ArbNet.Models.DTOs
{
    // Registro
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string? Country { get; set; }
    }

    // Login
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    // Respuesta
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? UserID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}