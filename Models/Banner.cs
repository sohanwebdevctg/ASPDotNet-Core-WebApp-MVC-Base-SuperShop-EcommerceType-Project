using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }

        [StringLength(15, ErrorMessage = "Only 15 Character Support!")]
        public string? BannerType { get; set; }

        [StringLength(15, ErrorMessage = "Only 15 Character Support!")]
        public string? BannerName { get; set; }

        public string? BannerImage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
