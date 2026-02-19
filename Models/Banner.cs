using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }

        public string? BannerType { get; set; }

        public string? BannerName { get; set; }

        public string? BannerImage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
