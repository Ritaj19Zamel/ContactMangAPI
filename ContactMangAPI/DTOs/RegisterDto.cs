using System.ComponentModel.DataAnnotations;

namespace ContactMangAPI.DTOs
{
    public class RegisterDto
    {
        [Required, EmailAddress] 
        public string Email { get; set; } = null!;
        [Required, MinLength(6)] 
        public string Password { get; set; } = null!;
        [Required] 
        public string FullName { get; set; } = null!;
    }
}
