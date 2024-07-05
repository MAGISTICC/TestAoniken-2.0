using Microsoft.EntityFrameworkCore;
using TestAoniken.Models;

namespace TestAoniken.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Publicacion> Publicaciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
