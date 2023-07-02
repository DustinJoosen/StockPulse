
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPulse.Models
{
    public class OrderLine
    {
        [Column("order_num")]
        [ForeignKey("order_num")]
        public int OrderNum { get; set; }
        
        [ForeignKey("OrderNum")]
        public Order Order { get; set; }


        [Column("product_num")]
        [ForeignKey("product_num")]
        public int ProductNum { get; set; }

        [ForeignKey("ProductNum")]
        public Product Product { get; set; }


        [Required]
        [ForeignKey("warehouse")]
        [Column("warehouse")]
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }

    }
}
