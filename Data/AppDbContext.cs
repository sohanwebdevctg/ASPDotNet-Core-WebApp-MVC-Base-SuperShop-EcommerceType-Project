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

    }
}
