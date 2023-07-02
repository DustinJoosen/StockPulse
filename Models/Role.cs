using System.ComponentModel.DataAnnotations;

namespace StockPulse.Models
{
    public class Role
    {
        [Key]
        public string Name { get; set; }

        public Role(string name)
        {
            this.Name = name;
        }
    }
}
