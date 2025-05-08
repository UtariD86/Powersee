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
        //builder.Property(pv => pv.personelId).HasColumnName("personelId").IsRequired();
        builder.Property(pv => pv.vardiyaId).HasColumnName("vardiyaId").IsRequired();
        builder.Property(pv => pv.baslangicZamani).HasColumnName("baslangicZamani").IsRequired();
        builder.Property(pv => pv.bitisZamani).HasColumnName("bitisZamani").IsRequired();

        builder.Property(d => d.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(d => d.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(d => d.DeletedDate).HasColumnName("DeletedDate");


        builder.HasData(GenerateSeedData());
    }

    private List<PlanlanmisVardiya> GenerateSeedData()
    {
        Random rand = new Random();
        var vardiyaIds = Enumerable.Range(1, 100).ToList(); // Foreign keyler 1-100 arasında olacak.
        var startDate = DateTime.Today.AddDays(-30); // Geçen ay
        var endDate = DateTime.Today.AddMonths(1); // Gelecek ay

        var planlanmisVardiyalar = new List<PlanlanmisVardiya>();

        // Bugün en az 3 vardiya ekle
        for (int i = 1; i <= 3; i++) // Burada ID'yi manuel olarak artırıyoruz
        {
            var vardiya = new PlanlanmisVardiya
            {
                Id = i, // ID'yi manuel olarak atıyoruz
                vardiyaId = vardiyaIds[rand.Next(0, vardiyaIds.Count)],
                baslangicZamani = DateTime.Today.AddHours(rand.Next(0, 24)),
                bitisZamani = DateTime.Today.AddHours(rand.Next(5, 9)),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            planlanmisVardiyalar.Add(vardiya);
        }

        // Geçen ay, bu ay, gelecek ay için rastgele vardiyalar ekle
        for (int i = 4; i <= 103; i++) // Devam eden ID sırasına göre ekliyoruz
        {
            var randomDate = startDate.AddDays(rand.Next(0, (endDate - startDate).Days)); // Rastgele bir tarih
            var vardiya = new PlanlanmisVardiya
            {
                Id = i, // ID'yi manuel olarak atıyoruz
                vardiyaId = vardiyaIds[rand.Next(0, vardiyaIds.Count)],
                baslangicZamani = randomDate.AddHours(rand.Next(0, 24)),
                bitisZamani = randomDate.AddHours(rand.Next(5, 9)),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            planlanmisVardiyalar.Add(vardiya);
        }

        return planlanmisVardiyalar;
    }

}


