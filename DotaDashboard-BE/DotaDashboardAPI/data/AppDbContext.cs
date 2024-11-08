using Microsoft.EntityFrameworkCore;
using DotaDashboardAPI.Models;

namespace DotaDashboardAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Hero> Heroes { get; set; }
    }
}
