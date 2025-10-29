using MapProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MapProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<GeoEntity> GeoEntities { get; set; }

        // Geometry konfigürasyonu YOK - sadece normal string/timestamp alanları
    }
}
