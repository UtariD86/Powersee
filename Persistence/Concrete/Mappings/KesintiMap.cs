using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Concrete.Mappings;

public class KesintiMap : IEntityTypeConfiguration<Kesinti>
{
    public void Configure(EntityTypeBuilder<Kesinti> builder)
    {
        builder.ToTable("Kesintiler").HasKey(k => k.Id);

        builder.Property(k => k.Id).HasColumnName("Id").IsRequired();
        builder.Property(k => k.PersonelId).HasColumnName("personelId").IsRequired(); 
        builder.Property(k => k.UygulanacakTarih).HasColumnName("uygulanacakTarih").HasColumnType("date").IsRequired();
        builder.Property(k => k.CezaMiktari).HasColumnName("cezaMiktari").HasColumnType("decimal(10, 2)").IsRequired();
        builder.Property(k => k.PlanlanmisVardiyaSnapshotId).HasColumnName("planlanmisVardiyaSnapshotId").IsRequired();

        builder.Property(k => k.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(k => k.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(k => k.DeletedDate).HasColumnName("DeletedDate");


        // Seed verisi
        var random = new Random();
        var kesintiler = new List<Kesinti>();

        for (int i = 1; i <= 100; i++)
        {
            kesintiler.Add(new Kesinti
            {
                Id = i,
                PersonelId = random.Next(1, 101),
                UygulanacakTarih = new DateTime(2024, 1, 1).AddDays(random.Next(0, 180)),
                CezaMiktari = Math.Round((decimal)(random.NextDouble() * 500 + 50), 2), // 50 ile 550 arasında rastgele
                PlanlanmisVardiyaSnapshotId = random.Next(1, 101),
                CreatedDate = DateTime.Now
            });
        }

        builder.HasData(kesintiler);
    }
}