using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        // Natigation Properties(Relation)
        public virtual ICollection<User>? Users { get; set; }
    }
}
