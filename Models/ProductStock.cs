using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPulse.Models
{
    public class ProductStock
    {
        [Column("warehouse")]
        [ForeignKey("warehouse")]
        public int WareHouseId { get; set; }

        [ForeignKey("WareHouseId")]
        public Warehouse Warehouse { get; set; }


        [Column("product_num")]
        [ForeignKey("product_num")]
        public int ProductNum { get; set; }

        [ForeignKey("ProductNum")]
        public Product Product { get; set; }

        public int? Quantity { get; set; }


    }
}
