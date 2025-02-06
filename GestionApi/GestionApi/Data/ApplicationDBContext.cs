using GestionApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionApi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
        }

        public DbSet<Carts> Carts { get; set; }
    }
}
