using StockPulse.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPulse.Models
{
    public class Employee : ICanContainImage
    {
        [Key]
        [Column("person")]
        [ForeignKey("person")]
        public string PersonEmail { get; set; }

        [ForeignKey("PersonEmail")]
        public Person Person { get; set; }


        [Column("montly_salary")]
        [Required]
        public double MonthlySalary { get; set; }

        [Column("employee_since")]
        public DateTime EmployeeSince { get; set; } = DateTime.UtcNow;

        [Column("profile_picture_path")]
        public string? ProfilePicturePath { get; set; }

        [Column("salted_password")]
        [Required]
        public string SaltedPassword { get; set; }

        public virtual ICollection<EmployeeRole> Roles { get; set; }

        [NotMapped]
        [Display(Name = "Profile picture")]
        public IFormFile? FormFile { get; set; }

        public Warehouse Warehouse { get; set; }

        [NotMapped]
        public bool IsAdmin
        {
            get
            {
                foreach (var role in Roles)
                {
                    if (role.Name.Equals("admin"))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        [NotMapped]
        public string FullImagePath
        {
            get
            {
                return (this.GetImagePath() == null)
                    ? "/lib/uploads/notfound.png"
                    : "/lib/uploads/employee/" + this.GetImagePath();
            }
        }

        public string GetImagePath()
        {
            return this.ProfilePicturePath;
        }

        public void SetImagePath(string imagePath)
        {
            this.ProfilePicturePath = imagePath;
        }

        public override string ToString()
        {
            return $"Person ({this.PersonEmail}";
        }
    }
}
