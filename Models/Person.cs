using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPulse.Models
{
    public class Person
    {
        [Key]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }

        public string? Particle { get; set; }

        [Required]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Display(Name = "Phonenumber")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string Pronouns { get; set; }

        [NotMapped]
        [Display(Name = "Full name")]
        public string FullName
        {
            get
            {
                return (this.Particle == null)
                    ? this.FirstName + " " + this.Lastname
                    : this.FirstName + " " + this.Particle + " " + this.Lastname;


            }
        }
    }
}
