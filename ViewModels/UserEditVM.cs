using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Models;

namespace SuperShop.ViewModels
{
    public class UserEditVM
    {

        // To store the user's original data
        public User UserData { get; set; } = new User();

        // SelectList for dropdown lists

        public IEnumerable<SelectListItem>? GenderList { get; set; }
        public IEnumerable<SelectListItem>? CityList { get; set; }
        public IEnumerable<SelectListItem>? CountryList { get; set; }
        public IEnumerable<SelectListItem>? RoleList { get; set; }

    }
}
