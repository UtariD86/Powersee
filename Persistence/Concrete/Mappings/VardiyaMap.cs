using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Concrete.Mappings;

public class VardiyaMap : IEntityTypeConfiguration<Vardiya>
{
    public void Configure(EntityTypeBuilder<Vardiya> builder)
    {
        builder.ToTable("Vardiyalar").HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("Id").IsRequired();
        builder.Property(v => v.vardiyaIsmi).HasColumnName("VardiyaIsmi").IsRequired(); 
        builder.Property(v => v.baslangicSaati).HasColumnName("BaslangicSaati").IsRequired();
        builder.Property(v => v.calismaSuresi).HasColumnName("CalismaSuresi").IsRequired();
        builder.Property(v => v.aciklama).HasColumnName("Aciklama").HasMaxLength(200); 
        builder.Property(v => v.listelenecek).HasColumnName("Listelenecek");
        builder.Property(v => v.ucretKatsayisi).HasColumnName("UcretKatsayisi").HasColumnType("decimal(5, 2)").IsRequired();
        builder.Property(v => v.esneklikPayiSuresi).HasColumnName("EsneklikPayiSuresi").IsRequired();

        var vardiyaIsimleri = new[] { "Gündüz Vardiyası", "Gece Vardiyası", "Ekip Vardiyası", "İkili Vardiya", "Dönüşümlü Vardiya" };
        var aciklamalar = new[] {
            "Standart 8 saatlik mesai.",
            "Gece saatlerinde yapılan çalışma.",
            "Ekip çalışması yapılan vardiya.",
            "Gündüz ve gece arasında dönüşümlü çalışma.",
            "Haftada 3 gün dönüşümlü olarak uygulanan vardiya."
        };

        var random = new Random();
        var vardiyalar = new List<Vardiya>();

        for (int i = 1; i <= 10; i++)  // 10 örnek vardiya ekledim
        {
            var vardiyaIsmi = vardiyaIsimleri[random.Next(vardiyaIsimleri.Length)];
            var baslangicSaati = new TimeOnly(random.Next(6, 22), random.Next(0, 60), 0);  // Başlangıç saati 6-22 arasında olacak
            var calismaSuresi = TimeSpan.FromHours(random.Next(7, 13));  // Çalışma süresi 7-12 saat arasında olacak
            var aciklama = aciklamalar[random.Next(aciklamalar.Length)];
            var ucretKatsayisi = random.Next(100, 201) / 100m;  // Ucret katsayısı 1.00 ile 2.00 arasında olacak
            var esneklikPayiSuresi = TimeSpan.FromHours(random.Next(0, 3));  // Esneklik payı süresi 0-2 saat arasında olacak

            vardiyalar.Add(new Vardiya
            {
                Id = i,
                vardiyaIsmi = vardiyaIsmi,
                baslangicSaati = baslangicSaati,
                calismaSuresi = calismaSuresi,
                aciklama = aciklama,
                listelenecek = i % 2 == 0,  // Vardiya her ikinci eleman için listelenecek olarak işaretlendi
                ucretKatsayisi = ucretKatsayisi,
                esneklikPayiSuresi = esneklikPayiSuresi
            });
        }

        builder.HasData(vardiyalar);
    }
}


