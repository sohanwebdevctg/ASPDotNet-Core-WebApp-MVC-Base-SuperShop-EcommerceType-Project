using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string? UserName { get; set; }

        [Required(ErrorMessage = "Please enter your valid email")]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Password must be at least 10 characters long")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Password must be 1 letter, 1 number & 1 special character")]
        public string UserPassword { get; set; }

        public string? UserImage { get; set; }

        public int? UserAge { get; set; }

        public string? UserStatus { get; set; } = "active";

        public string? UserAddress { get; set; }

        // Foreign key & Natigation Properties (Relation)

        public int? GenderId { get; set; }

        public virtual Gender? Gender { get; set; }

        public int? RoleId { get; set; } = 2;

        public virtual Role? Role { get; set; }

        public int? CityId { get; set; }

        public virtual City? City { get; set; }

        public int? CountryId { get; set; }

        public virtual Country? Country { get; set; }

    }
}
