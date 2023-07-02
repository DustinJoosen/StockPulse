using StockPulse.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockPulse.Models
{
    public class Product : ICanContainImage
    {
        [Key]
        [Column("product_num")]
        [Display(Name = "Product num")]
        public int ProductNum { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column("purchase_price")]
        [Display(Name = "Purchase Price")]
        public double PurchasePrice { get; set; }

        [Required]
        [Column("selling_price")]
        [Display(Name = "Selling Price")]
        public double SellingPrice { get; set; }

        [Column("image_path")]
        [Display(Name = "Image")]
        public string? ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile? FormFile { get; set; }

        [NotMapped]
        public int TotalStock => (this.ProductStocks == null)
            ? -1
            : (int)this.ProductStocks.Select(ps => ps.Quantity).Sum();

        [NotMapped]
        public double ProfitPerSell => this.SellingPrice - this.PurchasePrice;


        [NotMapped]
        public string FullImagePath
        {
            get
            {
                return (this.ImagePath == null)
                    ? "/lib/uploads/notfound.png"
                    : "/lib/uploads/product/" + this.ImagePath;
            }
        }


        public virtual ICollection<ProductStock> ProductStocks { get; set; }


        public string GetImagePath()
        {
            return this.ImagePath;
        }

        public void SetImagePath(string imagePath)
        {
            this.ImagePath = imagePath;
        }
    }
}
