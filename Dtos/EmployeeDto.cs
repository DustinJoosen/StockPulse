using StockPulse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPulse.Dtos
{
    public class EmployeeDto
    {

        [Key]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }

        public string? Particle { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Display(Name = "Phone-number")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string Pronouns { get; set; }

        [Required]
        [Display(Name = "Monthly Salary (€)")]
        public double MonthlySalary { get; set; }

        [Display(Name = "Employeed since")]
        public DateTime EmployeeSince { get; set; } = DateTime.UtcNow;

        [Display(Name = "Profile picturw")]
        public string? ProfilePicturePath { get; set; }

        [Display(Name = "Profile picture")]
        public IFormFile? FormFile { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string SaltedPassword { get; set; }

        public bool IsAdmin { get; set; } = false;

        [NotMapped]
        public string FullImagePath
        {
            get
            {
                return (this.ProfilePicturePath == null)
                    ? "/lib/uploads/notfound.png"
                    : "/lib/uploads/employee/" + this.ProfilePicturePath;
            }
        }

        public Person ExtractPerson()
        {
            return new Person
            {
                Email = this.Email,
                FirstName = this.FirstName,
                Particle = this.Particle,
                Lastname = this.Lastname,
                PhoneNumber = this.PhoneNumber,
                Pronouns = this.Pronouns
            };
        }

        public Employee ExtractEmployee()
        {
            return new Employee
            {
                PersonEmail = this.Email,
                EmployeeSince = this.EmployeeSince,
                MonthlySalary = this.MonthlySalary,
                ProfilePicturePath = this.ProfilePicturePath,
                FormFile = this.FormFile,
                SaltedPassword = this.SaltedPassword
            };
        }


        public static EmployeeDto Combine(Employee employee, Person person)
        {
            return new EmployeeDto
            {
                Email = person.Email,
                FirstName = person.FirstName,
                Particle = person.Particle,
                Lastname = person.Lastname,
                PhoneNumber = person.PhoneNumber,
                Pronouns = person.Pronouns,
                EmployeeSince = employee.EmployeeSince,
                MonthlySalary = employee.MonthlySalary,
                ProfilePicturePath = employee.ProfilePicturePath,
                SaltedPassword = employee.SaltedPassword,
                IsAdmin = employee.IsAdmin
            };
        }

    }
}

      