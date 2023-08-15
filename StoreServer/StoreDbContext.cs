using Microsoft.EntityFrameworkCore;
using StoreServer.DatabaseModels;

namespace StoreServer
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) :
            base(options)
        {

        }
        public DbSet<ORDER> Orders { get; set; }
        public DbSet<ORDERDETAILS> OrderDetails { get; set; }
        public DbSet<STORAGE> Storage { get; set; }
    }
}
