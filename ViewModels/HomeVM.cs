using SuperShop.Models;

namespace SuperShop.ViewModels
{
    public class HomeVM
    {

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable<Offer> Offers { get; set; }

    }
}
