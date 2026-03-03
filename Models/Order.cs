using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserImage { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public decimal TotalAmount { get; set; }

    }
}
