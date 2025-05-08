using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Persistence.Concrete.Mappings;

public class PersonelMap : IEntityTypeConfiguration<Personel>
{
    private static Random _random = new Random();
    public void Configure(EntityTypeBuilder<Personel> builder)
    {
        builder.ToTable("Personels").HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("Id");
        builder.Property(d => d.isim).HasColumnName("isim").IsRequired();
        builder.Property(d => d.soyisim).HasColumnName("soyisim").IsRequired();
        builder.Property(d => d.adres).HasColumnName("adres");
        builder.Property(d => d.telefonNumarasi1).HasColumnName("telefonNumarasi1").IsRequired();
        builder.Property(d => d.telefonNumarasi2).HasColumnName("telefonNumarasi2");
        builder.Property(d => d.tcKimlik).HasColumnName("tcKimlik").IsRequired();
        builder.Property(d => d.bankaHesapNo).HasColumnName("bankaHesapNo");
        builder.Property(d => d.vergiNo).HasColumnName("vergiNo");
        builder.Property(d => d.vergiDairesiAdi).HasColumnName("vergiDairesiAdi");
        builder.Property(d => d.aciklama).HasColumnName("aciklama");

        builder.Property(d => d.profilFotografiUrl).HasColumnName("profilFotografiUrl");

        builder.Property(d => d.departmanId).HasColumnName("departmanId");
        builder.Property(d => d.pozisyonId).HasColumnName("pozisyonId");
        builder.Property(d => d.subeId).HasColumnName("subeId");
        builder.Property(d => d.yillikIzinGunSayisi).HasColumnName("yillikIzinGunSayisi");
        builder.Property(d => d.performansNotu).HasColumnName("performansNotu");
        builder.Property(d => d.sgkSicilNo).HasColumnName("sgkSicilNo").IsRequired();
        
        builder.Property(d => d.haftalikSaat).HasColumnName("haftalikSaat").IsRequired();
        builder.Property(d => d.saatlikUcret).HasColumnName("saatlikUcret");

        builder.Property(d => d.dogumTarihi).HasColumnName("dogumTarihi").IsRequired();
        builder.Property(d => d.baslangicTarihi).HasColumnName("baslangicTarihi").IsRequired();
        builder.Property(d => d.bitisTarihi).HasColumnName("bitisTarihi");

        builder.Property(d => d.fazlaMesaiUygun).HasColumnName("fazlaMesaiUygun");
        builder.Property(d => d.Code).HasColumnName("Code");

        builder.Property(d => d.CalismaTipi).HasColumnName("CalismaTipi").IsRequired();
        builder.Property(d => d.Cinsiyet).HasColumnName("Cinsiyet").IsRequired();
        builder.Property(d => d.VardiyaTuru).HasColumnName("VardiyaTuru").IsRequired();












        builder.Property(d => d.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(d => d.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(d => d.DeletedDate).HasColumnName("DeletedDate");

        builder.HasData(GenerateSeedData());
    }
    private string? KodOlustur(string soyad)
    {
        // İlk 5 karakteri al, eksikse 'X' ile tamamla
        string ilkBes = soyad.ToUpper().PadRight(5, 'X').Replace("Ç", "C").Replace("ç", "c")
        .Replace("Ğ", "G").Replace("ğ", "g")
        .Replace("İ", "I").Replace("ı", "i")
        .Replace("Ö", "O").Replace("ö", "o")
        .Replace("Ş", "S").Replace("ş", "s")
        .Replace("Ü", "U").Replace("ü", "u").Substring(0, 5);

        // Rastgele 5 karakter oluştur
        string rastgeleKisim = RastgeleKodOlustur(5);

        // Birleştir
        string kod = $"{ilkBes}-{rastgeleKisim}";

        return kod;
    }

    public string RastgeleKodOlustur(int uzunluk)
    {
        const string karakterler = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < uzunluk; i++)
        {
            int index = random.Next(karakterler.Length);
            sb.Append(karakterler[index]);
        }

        return sb.ToString();
    }

    private List<Personel> GenerateSeedData()
    {
        var list = new List<Personel>();

        // 50 farklı isim ve soyisim - Tek indeksler erkek, çift indeksler kadın
        var isimler = new List<string>
    {
        "Mehmet", "Zeynep", "Faik", "Ayşe", "Tolga", "Emine", "Hüseyin", "Fatma", "Murat", "Elif",
        "Hasan", "Seda", "Kemal", "Zeynep", "Mustafa", "Ayşe", "Murat", "Seda", "Deniz", "Büşra",
        "Cem", "Havva", "Burak", "Gizem", "Emre", "Sibel", "Murat", "Zeynep", "Kaan", "Rabia",
        "Gökhan", "Aylin", "Can", "Derya", "Serkan", "Eda", "Kemal", "Neslihan", "Ömer", "Büşra",
        "Murat", "Cemre", "İsmail", "Tuğba", "Hakan", "Rüya", "Serkan", "Selin", "Baran", "Ayşegül"
    };

        var soyisimler = new List<string>
    {
        "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Polat", "Karadeniz", "Öztürk", "Kocabaş", "Koç",
        "Aydın", "Güzel", "Aksoy", "Yıldız", "Sarı", "Beyaz", "Ünal", "Arslan", "Işık", "Güneş",
        "Celik", "Sevgi", "Aydoğan", "Kılıç", "Çetin", "Başaran", "Yalçın", "Tekin", "Özdemir", "Türkmen",
        "Çakır", "Erdem", "Savaş", "Balcı", "Mert", "Çakmak", "Erdoğan", "Demirtaş", "Keskin", "Aslan",
        "Yüksek", "Toprak", "Karaca", "Akın", "İnce", "Gök", "Kurtuluş", "Sevim", "Kurt", "Özkan"
    };

        for (int i = 1; i <= 100; i++)
        {
            var isimIndex = _random.Next(isimler.Count);
            var soyisimIndex = _random.Next(soyisimler.Count);
            var isim = isimler[isimIndex];
            var soyisim = soyisimler[soyisimIndex];
            var code = KodOlustur(soyisim);
            // Tek indexler için erkek, çift indexler için kadın cinsiyeti belirle
            Cinsiyet cinsiyet = (isimIndex % 2 == 0) ? Cinsiyet.Erkek : Cinsiyet.Kadin;

            list.Add(new Personel
            {
                Id = i,
                isim = isim,
                soyisim = soyisim,
                adres = $"Adres {i}",
                telefonNumarasi1 = $"0500{i:D7}",
                telefonNumarasi2 = $"0555{i:D7}",
                tcKimlik = $"{_random.Next(100000000, 999999999)}{_random.Next(10)}",
                bankaHesapNo = $"TR{i:D2}0000000000000000{i:D4}",
                vergiNo = $"{_random.Next(100000000, 999999999)}",
                vergiDairesiAdi = $"Vergi Dairesi {i}",
                aciklama = $"Açıklama {i}",
                profilFotografiUrl = $"https://fakeimg.pl/100x100/?text=User{i}",
                departmanId = _random.Next(1, 101),
                pozisyonId = _random.Next(1, 101),
                subeId = _random.Next(1, 101),
                yillikIzinGunSayisi = _random.Next(0, 30),
                performansNotu = _random.Next(1, 101),
                sgkSicilNo = i,
                haftalikSaat = _random.Next(30, 50),
                saatlikUcret = _random.Next(100, 500),
                dogumTarihi = new DateTime(1990, 1, 1).AddDays(_random.Next(1000, 10000)),
                baslangicTarihi = new DateTime(2020, 1, 1).AddDays(_random.Next(0, 1500)),
                bitisTarihi = null, // isteğe göre eklenebilir
                fazlaMesaiUygun = _random.Next(0, 2) == 1,
                CalismaTipi = (CalismaTipi)_random.Next(1, Enum.GetValues(typeof(CalismaTipi)).Length),
                Cinsiyet = cinsiyet,
                VardiyaTuru = (VardiyaTuru)_random.Next(1,Enum.GetValues(typeof(VardiyaTuru)).Length),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                DeletedDate = null,
                Code= code
            });
        }

        return list;
    }

}


