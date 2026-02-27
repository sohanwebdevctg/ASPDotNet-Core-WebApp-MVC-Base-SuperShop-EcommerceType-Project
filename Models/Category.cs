
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage ="Category Name is Required")]
        [StringLength(25)]
        public string CategoryName { get; set; }

        // Natigation Properties(Relation)
        public virtual ICollection<Product>? Products { get; set; }

    }
}
