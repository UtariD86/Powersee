using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Concrete.Mappings;

public class DepartmentMap : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("Id").IsRequired();
        builder.Property(d => d.Name).HasColumnName("Name").IsRequired();
        builder.Property(d => d.Description).HasColumnName("Description");
        builder.Property(d => d.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(d => d.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(d => d.DeletedDate).HasColumnName("DeletedDate");
        builder.Property(d => d.Adres).HasColumnName("Adres").IsRequired();
        builder.Property(d => d.Managerid).HasColumnName("Managarıd");
        builder.Property(d => d.Active).HasColumnName("Active");
        builder.Property(d => d.UniqueCode).HasColumnName("UniqueCode");
        builder.Property(d => d.CalismaTuru).HasColumnName("CalismaTuru");




    }
}