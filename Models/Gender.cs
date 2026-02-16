using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }

        public string GenderName { get; set; }

        // Natigation Properties(Relation)

        public virtual ICollection<User>? Users { get; set; }

    }
}
