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
        }
    }
}
