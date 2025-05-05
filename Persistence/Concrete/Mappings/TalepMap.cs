using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Concrete.Mappings
{
    public class TalepMap : IEntityTypeConfiguration<Talep>
    {
        public void Configure(EntityTypeBuilder<Talep> builder)
        {
            builder.Property(d => d.Durum).HasColumnName("Durum").IsRequired();
            builder.Property(d => d.aciklama).HasColumnName("aciklama").IsRequired();
            builder.Property(d => d.gondericiId).HasColumnName("gondericiId");
            builder.Property(d => d.planlanmisVardiyaId).HasColumnName("planlanmisVardiyaId").IsRequired();
            builder.Property(d => d.TalepTuru).HasColumnName("TalepTuru");
            builder.Property(d => d.aliciId).HasColumnName("AliciId");
            builder.Property(i => i.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(i => i.UpdatedDate).HasColumnName("UpdatedDate");
            builder.Property(i => i.DeletedDate).HasColumnName("DeletedDate");
        }
    }
}
