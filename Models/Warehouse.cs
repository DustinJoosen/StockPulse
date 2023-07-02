using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPulse.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Zipcode")]
        public string ZipCode { get; set; }
        
        [Required]
        public string City { get; set; }

        [Required]
        [Column("manager")]
        [ForeignKey("manager")]
        [Display(Name = "Manager")]
        public string ManagerEmail { get; set; }

        [ForeignKey("ManagerEmail")]
        public Employee Manager { get; set; }

        public virtual ICollection<ProductStock> ProductStocks { get; set; }
    }
}
