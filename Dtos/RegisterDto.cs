using System.ComponentModel.DataAnnotations;

namespace StockPulse.Dtos
{
    public class RegisterDto
    {
        [Key]
        public string Email { get; set; }

        [Required]
        [Display (Name = "First name")]
        public string FirstName { get; set; }

        public string? Particle { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Display (Name = "Phonenumber")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string Pronouns { get; set; }

        [Display (Name = "Profile picture")]

        public string? ProfilePicturePath { get; set; }

        [Required]
        [Display (Name = "Password")]
        public string SaltedPassword { get; set; }

        [Required]
        [Display(Name = "Password confirmation")]
        public string SaltedPasswordConfirmation { get; set; }

        [Display(Name = "Profile picture")]
        public IFormFile? FormFile { get; set; }


    }
}
