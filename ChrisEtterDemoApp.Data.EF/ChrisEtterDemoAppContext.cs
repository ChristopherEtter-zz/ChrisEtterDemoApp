using ChrisEtterDemoApp.Data.EF.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChrisEtterDemoApp.Data.EF
{
    public class ChrisEtterDemoAppContext : IdentityDbContext<StoreUser>
    {
        public ChrisEtterDemoAppContext(DbContextOptions<ChrisEtterDemoAppContext> options)
           : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
