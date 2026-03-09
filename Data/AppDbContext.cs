using Microsoft.EntityFrameworkCore;
using SuperShop.Models;

namespace SuperShop.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // models table here
        public DbSet<Gender> Genders { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Banner> Banners { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Category> Categoreis { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<Payment> Payments { get; set; }

    }
}
