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

public class DepartmentMap : IEntityTypeConfiguration<Department>
{
    private static Random _random = new Random();

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

        // Seed Data Ekleme
        builder.HasData(GenerateSeedData());
    }

    private string KodOlustur(string departmanAdi)
    {
        // İlk 5 karakteri al, eksikse 'X' ile tamamla
        string ilkBes = departmanAdi.ToUpper().PadRight(5, 'X').Substring(0, 5);

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



    private List<Department> GenerateSeedData()
    {
        // Anlamlı departman isimlerini içeren liste
        var departmentNames = new List<string>
        {
            "İnsan Kaynakları", "Finans", "Pazarlama", "Bilgi Teknolojileri", "Hukuk",
            "Satın Alma", "Müşteri Hizmetleri", "Üretim", "Ar-Ge", "Lojistik",
            "Tedarik Zinciri", "Eğitim", "İletişim", "Satış", "Halkla İlişkiler",
            "Yönetim", "Strateji", "İç Denetim", "Kalite Kontrol", "Ekip Yönetimi",
            "Projeler", "Dijital Pazarlama", "Yazılım Geliştirme", "Veri Analizi",
            "İnternet Güvenliği", "Makine Bakım", "Mühendislik", "İnovasyon",
            "Yatırım", "Sosyal Medya", "İnsan Kaynakları Eğitim", "Dış İlişkiler",
            "Satış ve Dağıtım", "E-ticaret", "Finansal Raporlama", "İç İletişim",
            "Ticaret", "İç İletişim ve Kültür", "Hizmet Yönetimi", "İdari İşler",
            "Planlama", "Pazarlama İletişimi", "Müşteri Memnuniyeti", "Reklam",
            "Bilişim Sistemleri", "İletişim ve Medya", "Yatırım ve Finans",
            "Stratejik Planlama", "Yönetim Danışmanlığı", "Ekip İletişimi",
            "Sosyal Sorumluluk", "Dijital Dönüşüm", "Veritabanı Yönetimi",
            "Bütçe ve Tahmin", "Risk Yönetimi", "İç Kontrol", "Kamu İlişkileri",
            "Kurumsal İletişim", "Teknik Destek", "Proje Yönetimi", "İç Denetim ve Güvence",
            "Tedarik Yönetimi", "Marka Yönetimi", "Satış Stratejileri", "Teknolojik Araştırmalar",
            "Liderlik Gelişimi", "Girişimcilik", "İleri Teknolojiler", "Sistem Analizi",
            "Bilişim Güvenliği", "İş Geliştirme", "Veri Bilimi", "Stratejik Pazarlama",
            "Yazılım Mühendisliği", "E-ticaret ve Pazarlama", "Satış Eğitim",
            "Kurumsal Satış", "Çalışan İlişkileri", "Sektör Analizi", "Hizmet İnovasyonu",
            "İnsan Kaynakları Stratejileri", "Kariyer Gelişimi", "Yeni İş Geliştirme",
            "Kurumsal İnovasyon", "Bilişim ve Telekomünikasyon", "Ağ Yönetimi",
            "Sosyal Medya İletişimi", "İşletme ve Yönetim", "Strateji ve Operasyon",
            "Yatırımcı İlişkileri", "Ürün Yönetimi", "İş Zekası", "Veri Yönetimi",
            "Pazar Araştırması", "İş Sürekliliği", "Proje Planlama", "Sistem Tasarımı",
            "İşlemler Yönetimi", "Dijital Strateji", "Yenilikçi Teknolojiler",
            "Kurumsal Satış ve Pazarlama", "Müşteri İlişkileri Yönetimi", "Veri Analitiği"
        };

        var departments = new List<Department>();

        // Listeyi foreach ile döngüye alıp seed data olarak ekliyoruz
        int id = 1;
        foreach (var name in departmentNames)
        {
            departments.Add(new Department
            {
                Id = id,
                Name = name,
                Description = $"{name} departmanı açıklaması.",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                DeletedDate = null,
                Adres = $"Adres {name}",
                Managerid = _random.Next(1,101), // örnek olarak yöneticiler arasında döngü yapılıyor
                Active = true,
                UniqueCode = KodOlustur(name),
                CalismaTuru = (CalismaTuru)_random.Next(1,Enum.GetValues(typeof(CalismaTuru)).Length)
            });
            id++;
        }

        return departments;
    }
}