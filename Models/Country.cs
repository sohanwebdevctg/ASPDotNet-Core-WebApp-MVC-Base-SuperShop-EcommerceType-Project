using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        // Natigation Properties(Relation)
        public virtual ICollection<User>? Users { get; set; }
    }
}
