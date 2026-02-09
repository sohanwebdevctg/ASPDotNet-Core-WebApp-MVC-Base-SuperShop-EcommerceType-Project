using Microsoft.EntityFrameworkCore;

namespace SuperShop.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // model table here


    }
}
