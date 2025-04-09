using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Entities;
using Persistence.Concrete.Mappings;


namespace Persistence.Concrete;

/// <summary>
/// Veritabanı işlemleri için kullanılacak olan DbContext sınıfı.
/// </summary>
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Veritabanı tabloları buraya DbSet olarak eklenir.
    public DbSet<Department> Departments { get; set; }
    public DbSet<Domain.Entities.Position> Positions { get; set; }// NetTopologySuite.GeometriesGraph.Position diye birşey olduğu içn böyle yazdım




    public DbSet<Sube> Subeler { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tablo Konfigürasyonları için oluşturulmuş mapping sınıfları burada apply edilir.
        modelBuilder.ApplyConfiguration(new DepartmentMap());
        modelBuilder.ApplyConfiguration(new PositionMap());

        modelBuilder.ApplyConfiguration(new SubeMap());
    }
}
