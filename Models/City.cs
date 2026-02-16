using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        public string CityName { get; set; }

        // Natigation Properties(Relation)
        public virtual ICollection<User>? Users { get; set; }

    }
}
