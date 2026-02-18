using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class Contact
    {

        [Key]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Eamil Is Required")]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Message is Required")]
        public string UserMessage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
