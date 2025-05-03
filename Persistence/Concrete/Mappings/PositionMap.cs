using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Persistence.Concrete.Mappings
{
    public class PositionMap : IEntityTypeConfiguration<Position>
    {
        private static Random _random = new Random();

        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions"); // Veritabanı tablosu adı

            builder.HasKey(p => p.Id); // Birincil anahtar

            builder.Property(p => p.Code)
                   .IsRequired()
                   .HasMaxLength(11); // 5 harf + "-" + 5 rastgele karakter

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Salary)
              .HasColumnType("decimal(18, 2)") // Veritabanı sütun tipini belirtir
              .HasPrecision(18, 2) // Ondalık hassasiyetini belirtir
              .IsRequired(); // Gerekliyse

            builder.Property(p => p.ManagerId)
                   .IsRequired();

            builder.Property(p => p.Active)
                   .HasDefaultValue(true);


            builder.HasData(GenerateSeedData());
        }

        private List<Position> GenerateSeedData()
        {
            var positions = new List<Position>();

            for (int i = 1; i <= 100; i++)
            {
                var name = $"Pozisyon {i}";
                var code = KodOlustur(name);

                positions.Add(new Position
                {
                    Id = i,
                    Name = name,
                    Code = code,
                    Salary = _random.Next(10000, 50000),
                    ManagerId = _random.Next(1, 101),
                    DepartmentId = _random.Next(1, 101),
                    Active = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    DeletedDate = null,
                });
            }

            return positions;
        }

        private string KodOlustur(string pozisyonAdi)
        {
            string ilkBes = pozisyonAdi.ToUpper().PadRight(5, 'X').Substring(0, 5);
            string rastgeleKisim = RastgeleKodOlustur(5);
            return $"{ilkBes}-{rastgeleKisim}";
        }

        private string RastgeleKodOlustur(int uzunluk)
        {
            const string karakterler = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new();
            for (int i = 0; i < uzunluk; i++)
            {
                int index = _random.Next(karakterler.Length);
                sb.Append(karakterler[index]);
            }
            return sb.ToString();
        }
    }
}
