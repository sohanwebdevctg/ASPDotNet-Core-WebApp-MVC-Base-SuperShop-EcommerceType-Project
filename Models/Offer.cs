using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class Offer
    {

        [Key]
        public int OfferId { get; set; }

        [Required(ErrorMessage = "Offer Type is required")]
        [StringLength(10, ErrorMessage = "Only 10 Character Support!")]
        public string? OfferType { get; set; }

        [Required(ErrorMessage = "Offer Name is required")]
        [StringLength(10, ErrorMessage = "Only 10 Character Support!")]
        public string? OfferName { get; set; }

        [Required(ErrorMessage = "Offer Price is required")]
        [Range(0, 100000, ErrorMessage = "Price Must Be Between 0 To 100,000")]
        public decimal OfferPrice { get; set; }

        public string? OfferImage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}