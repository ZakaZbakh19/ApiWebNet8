using GestionApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionApi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
