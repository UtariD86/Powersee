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
    public class PlanlanmisVardiyaPersonelMap : IEntityTypeConfiguration<PlanlanmisVardiyaPersonel>
    {
        public void Configure(EntityTypeBuilder<PlanlanmisVardiyaPersonel> builder)
        {
            builder.Property(d => d.Id).HasColumnName("Id").IsRequired();
            builder.Property(d => d.PlanlanmisVardiyaId).HasColumnName("PlanlanmisVardiyaId").IsRequired();
            builder.Property(d => d.PersonelId).HasColumnName("PersonelId").IsRequired();
            builder.Property(d => d.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            builder.Property(d => d.UpdatedDate).HasColumnName("UpdatedDate");

            builder.Property(pv => pv.girisZamani).HasColumnName("girisZamani");
            builder.Property(pv => pv.cikisZamani).HasColumnName("cikisZamani");
            builder.Property(pv => pv.hedefUcret).HasColumnName("hedefUcret").HasColumnType("decimal(10, 2)");
            builder.Property(pv => pv.kazanilanUcret).HasColumnName("kazanilanUcret").HasColumnType("decimal(10, 2)");

            builder.Property(d => d.DeletedDate).HasColumnName("DeletedDate");
            // Seed verisi ekle
            builder.HasData(GenerateSeedData());
        }

        private List<PlanlanmisVardiyaPersonel> GenerateSeedData()
        {
            Random rand = new Random();
            var planlanmisVardiyaIds = Enumerable.Range(1, 100).ToList(); // Foreign keyler 1-100 arasında olacak.
            var personelIds = Enumerable.Range(1, 100).ToList(); // Foreign keyler 1-100 arasında olacak.

            var planlanmisVardiyaPersoneller = new List<PlanlanmisVardiyaPersonel>();

            // 100 rastgele ilişki oluştur
            for (int i = 1; i <= 100; i++) // id'yi 1'den başlatarak manuel olarak ekle
            {
                var planlanmisVardiyaPersonel = new PlanlanmisVardiyaPersonel
                {
                    Id = i, // Burada ID'yi manuel olarak belirliyoruz.
                    PlanlanmisVardiyaId = planlanmisVardiyaIds[rand.Next(0, planlanmisVardiyaIds.Count)], // Rastgele PlanlanmisVardiyaId
                    PersonelId = personelIds[rand.Next(0, personelIds.Count)], // Rastgele PersonelId
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                planlanmisVardiyaPersoneller.Add(planlanmisVardiyaPersonel);
            }

            return planlanmisVardiyaPersoneller;
        }

    }
}
