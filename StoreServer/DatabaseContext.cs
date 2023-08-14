using Microsoft.EntityFrameworkCore;
using StoreServer.DatabaseModels;

namespace StoreServer
{
    internal class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) :
            base(options)
        {

        }
        public DbSet<ORDER> Orders { get; set; }
        public DbSet<ORDERDETAILS> OrderDetails { get; set; }
        public DbSet<STORAGE> Storage { get; set; }
    }
}
