using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class Payment
    {

        [Key]
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string PaymentType { get; set; }
        public string AccountNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;

    }
}
