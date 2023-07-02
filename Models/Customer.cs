using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPulse.Models
{
    public class Customer
    {
        [Key]
        [Column("person")]
        [ForeignKey("person")]
        public string PersonEmail { get; set; }

        [ForeignKey("PersonEmail")]
        public Person Person { get; set; }

        public string? Street { get; set; }

        [Display (Name = "Zipcode")]
        public string? ZipCode { get; set; }
        
        public string? City { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public override string ToString()
        {
            return $"Customer ({this.PersonEmail}";
        }
    }
}
