using System.ComponentModel.DataAnnotations;

namespace dotnet.Models
{
    public class UserModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 6)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string HashedPassword { get; set; } = string.Empty;
        public string? Address { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public string? DataOfBirth { get; set; }
    }
}