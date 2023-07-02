using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPulse.Models
{

    public class EmployeeRole
    {
        [Key]
        public string EmployeeEmail { get; set; }

        [ForeignKey("EmployeeEmail")]
        public Employee Employee { get; set; }

        [Key]
        public string Name { get; set; }
    }
}
