using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPulse.Models
{
    public class Order
    {
        [Key]
        [Column("order_num")]
        [Display(Name = "Order Num")]
        public int OrderNum { get; set; }

        [Column("customer")]
        [ForeignKey("customer")]
        [Required]
        [Display(Name = "Customer Email")]
        public string CustomerEmail { get; set; }

        [ForeignKey("CustomerEmail")]
        public Customer? Customer { get; set; }

        [Column("discount_price")]
        [Display(Name = "Discount Price")]
        public double? DiscountPrice { get; set; } = 0.0;

        [Column("delivery_notes")]
        [Display(Name = "Delivery Notes")]
        public string? DeliveryNotes { get; set; }

        [NotMapped]
        [Display(Name = "Total Price")]
        public double TotalPrice
        {

            get 
            {
                try
                {
                    var products = this.OrderLines.Select(ol => ol.Product).ToList();

                    var total_price = products.Sum(p => p.SellingPrice);
                    var discount = this.DiscountPrice != null ? (double)this.DiscountPrice : 0.0;

                    return total_price - discount;
                } 
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        public virtual ICollection<OrderLine>? OrderLines { get; set; }

    }
}
