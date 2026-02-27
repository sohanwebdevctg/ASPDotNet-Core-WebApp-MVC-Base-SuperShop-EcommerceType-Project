using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperShop.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product Price is required")]
        [Range(0, 100000, ErrorMessage = "Price Must Be Between 0 To 100,000")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public decimal ProductPrice { get; set; }

        [Range(0, 5000, ErrorMessage = "Stock cannot be negative")]
        public int ProductLimit { get; set; }

        public string? ProductImage { get; set; }

        [StringLength(500, ErrorMessage = "Only 500 Character Support!")]
        public string? ProductDescription {get; set; }

        // Foreign key & Natigation Properties (Relation)

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}
