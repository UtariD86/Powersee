using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Concrete.Mappings;

public class PlanlanmisVardiyaMap : IEntityTypeConfiguration<PlanlanmisVardiya>
{
    public void Configure(EntityTypeBuilder<PlanlanmisVardiya> builder)
    {
        builder.ToTable("PlanlanmisVardiyalar").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("Id").IsRequired();
        builder.Property(pv => pv.personelId).HasColumnName("personelId").IsRequired();
        builder.Property(pv => pv.vardiyaId).HasColumnName("vardiyaId").IsRequired();
        builder.Property(pv => pv.baslangicZamani).HasColumnName("baslangicZamani").IsRequired();
        builder.Property(pv => pv.bitisZamani).HasColumnName("bitisZamani").IsRequired();
        builder.Property(pv => pv.girisZamani).HasColumnName("girisZamani");
        builder.Property(pv => pv.cikisZamani).HasColumnName("cikisZamani");
        builder.Property(pv => pv.hedefUcret).HasColumnName("hedefUcret").HasColumnType("decimal(10, 2)");
        builder.Property(pv => pv.kazanilanUcret).HasColumnName("kazanilanUcret").HasColumnType("decimal(10, 2)");
        builder.Property(d => d.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(d => d.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(d => d.DeletedDate).HasColumnName("DeletedDate");


    }
}


