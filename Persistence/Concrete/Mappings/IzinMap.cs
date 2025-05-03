using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Domain.Enums; // Doğru namespace'i kontrol edin

namespace Persistence.Concrete.Mappings
{


    public class IzinMap : IEntityTypeConfiguration<Izin>
    {
    private static Random _random = new Random();
        public void Configure(EntityTypeBuilder<Izin> builder)
        {
            builder.ToTable("Izinler").HasKey(i => i.Id); // Veritabanındaki tablo adı ve Primary Key

            builder.Property(i => i.Id).HasColumnName("Id").IsRequired(); // Gerekli ve sütun adı

            builder.Property(i => i.PersonelId)
                   .HasColumnName("PersonelId")
                   .IsRequired(); // Zorunlu alan ve sütun adı

            builder.Property(i => i.BaslangicTarihi)
                   .HasColumnName("BaslangicTarihi")
                   .IsRequired(); // Zorunlu alan ve sütun adı

            builder.Property(i => i.BitisTarihi)
                   .HasColumnName("BitisTarihi")
                   .IsRequired(); // Zorunlu alan ve sütun adı

            builder.Property(i => i.IzinTuruEnum)
                   .HasColumnName("IzinTuru")
                   .IsRequired(); // Zorunlu alan ve sütun adı

            builder.Property(i => i.UcretTuruEnum)
                   .HasColumnName("UcretTuru")
                   .IsRequired(); // Zorunlu alan ve sütun adı

            builder.Property(i => i.Aciklama)
                   .HasColumnName("Aciklama")
                   .HasMaxLength(500); // Max uzunluk ve sütun adı

            builder.Property(i => i.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(i => i.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(i => i.DeletedDate).HasColumnName("DeletedDate");


            // Seed verisi (1 ile 100 arasında random PersonelId)
            var random = new Random();
            var izinler = new List<Izin>();

            for (int i = 1; i <= 100; i++)
            {
                izinler.Add(new Izin
                {
                    Id = i,
                    PersonelId = random.Next(1, 101),
                    BaslangicTarihi = new DateTime(2024, 1, 1).AddDays(random.Next(0, 90)),
                    BitisTarihi = new DateTime(2024, 1, 1).AddDays(random.Next(91, 120)),
                    IzinTuruEnum = (IzinTuruEnum)_random.Next(1,Enum.GetValues(typeof(IzinTuruEnum)).Length), // örnek değer
                    UcretTuruEnum = (UcretTuruEnum)_random.Next(1,Enum.GetValues(typeof(UcretTuruEnum)).Length), // örnek değer
                    Aciklama = $"Seed ile eklenmiş izin {i}",
                    CreatedDate = DateTime.Now
                });
            }

            builder.HasData(izinler);
        }
    }
}