using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities; // Doğru namespace'i kontrol edin

namespace Persistence.Concrete.Mappings
{
    public class IzinMap : IEntityTypeConfiguration<Izin>
    {
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
        }
    }
}