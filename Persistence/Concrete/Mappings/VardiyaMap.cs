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


    }
}


