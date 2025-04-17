using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Concrete.Mappings;

public class PersonelMap : IEntityTypeConfiguration<Personel>
{
    public void Configure(EntityTypeBuilder<Personel> builder)
    {
        builder.ToTable("Personels").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("Id");
        builder.Property(d => d.isim).HasColumnName("isim");
        builder.Property(d => d.soyisim).HasColumnName("soyisim");
        builder.Property(d => d.adres).HasColumnName("adres");
        builder.Property(d => d.telefonNumarasi1).HasColumnName("telefonNumarasi1");
        builder.Property(d => d.telefonNumarasi2).HasColumnName("telefonNumarasi2");
        builder.Property(d => d.tcKimlik).HasColumnName("tcKimlik");
        builder.Property(d => d.bankaHesapNo).HasColumnName("bankaHesapNo");
        builder.Property(d => d.vergiNo).HasColumnName("vergiNo");
        builder.Property(d => d.vergiDairesiAdi).HasColumnName("vergiDairesiAdi");
        builder.Property(d => d.aciklama).HasColumnName("aciklama");

        builder.Property(d => d.departmanId).HasColumnName("departmanId");
        builder.Property(d => d.pozisyonId).HasColumnName("pozisyonId");
        builder.Property(d => d.subeId).HasColumnName("subeId");
        builder.Property(d => d.yillikIzinGunSayisi).HasColumnName("yillikIzinGunSayisi");
        builder.Property(d => d.performansNotu).HasColumnName("performansNotu");
        builder.Property(d => d.sgkSicilNo).HasColumnName("sgkSicilNo");
        
        builder.Property(d => d.haftalikSaat).HasColumnName("haftalikSaat");
        builder.Property(d => d.saatlikUcret).HasColumnName("saatlikUcret");

        builder.Property(d => d.dogumTarihi).HasColumnName("dogumTarihi");
        builder.Property(d => d.baslangicTarihi).HasColumnName("baslangicTarihi");
        builder.Property(d => d.bitisTarihi).HasColumnName("bitisTarihi");

        builder.Property(d => d.fazlaMesaiUygun).HasColumnName("fazlaMesaiUygun");

        builder.Property(d => d.CalismaTipi).HasColumnName("CalismaTipi");
        builder.Property(d => d.Cinsiyet).HasColumnName("Cinsiyet");
        builder.Property(d => d.VardiyaTuru).HasColumnName("VardiyaTuru");












        builder.Property(d => d.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(d => d.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(d => d.DeletedDate).HasColumnName("DeletedDate");


    }
}


