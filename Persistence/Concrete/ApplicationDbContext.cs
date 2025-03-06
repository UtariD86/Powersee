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
     
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tablo Konfigürasyonları için oluşturulmuş mapping sınıfları burada apply edilir.
        modelBuilder.ApplyConfiguration(new DepartmentMap());
    }
}
