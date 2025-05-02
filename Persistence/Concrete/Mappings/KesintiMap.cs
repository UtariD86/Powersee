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
    }
}