using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Models;

namespace SuperShop.ViewModels
{
    public class ProductVM
    {

        // To store the product original data

        public Product ProductData { get; set; } = new Product();

        // SelectList for dropdown lists

        public IEnumerable<SelectListItem>? CategoryList { get; set; }

    }
}
