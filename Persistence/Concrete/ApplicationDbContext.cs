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

    public DbSet<Personel> Personels { get; set; }
    public DbSet<Vardiya> Vardiyalar { get; set; }
    public DbSet<Izin> Izinler { get; set; }
    public DbSet<Kesinti> Kesintiler { get; set; }

    public DbSet<PlanlanmisVardiyaPersonel> PlanlanmisVardiyaPersoneller { get; set; }

    public DbSet<PlanlanmisVardiya> PlanlanmisVardiyalar { get; set; }


    public DbSet<Talep> Taleps { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tablo Konfigürasyonları için oluşturulmuş mapping sınıfları burada apply edilir.
        modelBuilder.ApplyConfiguration(new DepartmentMap());
        modelBuilder.ApplyConfiguration(new PositionMap());
        modelBuilder.ApplyConfiguration(new SubeMap());

        modelBuilder.ApplyConfiguration(new KesintiMap());
        modelBuilder.ApplyConfiguration(new IzinMap());
        modelBuilder.ApplyConfiguration(new PersonelMap());
        modelBuilder.ApplyConfiguration(new VardiyaMap());
        modelBuilder.ApplyConfiguration(new PlanlanmisVardiyaMap());
        modelBuilder.ApplyConfiguration(new TalepMap());
        modelBuilder.ApplyConfiguration(new PlanlanmisVardiyaPersonelMap());
    }
}