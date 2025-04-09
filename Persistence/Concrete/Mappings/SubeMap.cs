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


    }
}


