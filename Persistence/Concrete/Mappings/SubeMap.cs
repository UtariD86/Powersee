using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Concrete.Mappings;

public class SubeMap : IEntityTypeConfiguration<Sube>
{
    public void Configure(EntityTypeBuilder<Sube> builder)
    {
        builder.ToTable("Subeler").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("Id").IsRequired();
        builder.Property(d => d.Subeisim).HasColumnName("Subeisim").IsRequired();
        builder.Property(d => d.Adres).HasColumnName("Adres");
        builder.Property(d => d.TelefonNumarasi1).HasColumnName("TelefonNumarasi1");
        builder.Property(d => d.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(d => d.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(d => d.DeletedDate).HasColumnName("DeletedDate");

        var sehirler = new[] { "İstanbul", "Ankara", "İzmir", "Bursa", "Konya", "Adana", "Antalya", "Gaziantep", "Eskişehir", "Kayseri" };
        var subeTipleri = new[] { "Merkez", "Sanayi", "Teknopark", "Üniversite", "Organize", "AVM", "Bölge", "Ofis", "Depo", "Ar-Ge" };

        var random = new Random();
        var subeler = new List<Sube>();

        for (int i = 1; i <= 100; i++)
        {
            var sehir = sehirler[random.Next(sehirler.Length)];
            var tip = subeTipleri[random.Next(subeTipleri.Length)];

            subeler.Add(new Sube
            {
                Id = i,
                Subeisim = $"{sehir} {tip} Şubesi",
                Adres = $"{sehir} Mahallesi, {tip} Caddesi No:{i}",
                TelefonNumarasi1 = $"0{random.Next(500, 560)}{random.Next(1000000, 9999999)}",
                CreatedDate = DateTime.Now
            });
        }

        builder.HasData(subeler);
    }
}


