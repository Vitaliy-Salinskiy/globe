using System.ComponentModel.DataAnnotations;

namespace dotnet.Dtos
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(35, MinimumLength = 6)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? Address { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public string? DataOfBirth { get; set; }
    }
}