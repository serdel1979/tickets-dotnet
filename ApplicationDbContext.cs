using Microsoft.EntityFrameworkCore;
using tickets.Entidades;

namespace tickets
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Estado> Estados { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
    }
}
