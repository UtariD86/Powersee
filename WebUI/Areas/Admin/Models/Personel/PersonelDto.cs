using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Areas.Admin.Models.Personel
{





    public class PersonelDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        [MinLength(5, ErrorMessage = "İsim en az 3 karakter olmalıdır.")]
        [MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
        public string? isim { get; set; }
        [Required(ErrorMessage = "Soyisim alanı zorunludur.")]
        [MinLength(5, ErrorMessage = "Soyisim en az 3 karakter olmalıdır.")]
        [MaxLength(50, ErrorMessage = "Soyisim en fazla 50 karakter olabilir.")]
        public string? soyisim { get; set; }
        public string? adres { get; set; }
        
        [Required(ErrorMessage = "Telefon numarası alanı zorunludur.")]
        [MinLength(10, ErrorMessage = "Telefon numarası en az 10 karakter olmalıdır.")]
        [MaxLength(17, ErrorMessage = "Telefon numarası en fazla 16 karakter olabilir.")]
        [RegularExpression(@"^[\d +]+$", ErrorMessage = "Telefon numarası sadece sayı, boşluk ve + karakteri içerebilir.")]
        public string? telefonNumarasi1 { get; set; }
        
        [MinLength(10, ErrorMessage = "Telefon numarası en az 10 karakter olmalıdır.")]
        [MaxLength(17, ErrorMessage = "Telefon numarası en fazla 16 karakter olabilir.")]
        [RegularExpression(@"^[\d +]+$", ErrorMessage = "Telefon numarası sadece sayı, boşluk ve + karakteri içerebilir.")]
        public string? telefonNumarasi2 { get; set; }

        [Required(ErrorMessage = "TC. Kimlik No alanı zorunludur.")]
        [MinLength(11, ErrorMessage = "TC. Kimlik No 11 karakter uzunluğunda olmalıdır.")]
        [MaxLength(11, ErrorMessage = "TC. Kimlik No 11 karakter uzunluğunda olmalıdır.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TC. Kimlik No alanı sadece rakamlardan oluşabilir ve 11 karakter uzunluğunda olmalıdır.")]
        public string tcKimlik { get; set; }
        [RegularExpression(@"^[\d -]+$", ErrorMessage = "Banka Hesap No alanına sadece rakam ve '-' karakteri girilebilir")]
        public string? bankaHesapNo { get; set; }
        [RegularExpression(@"^[\d]+$", ErrorMessage = "Vergi No alanına sadece rakam girilebilir")]
        public string? vergiNo { get; set; }
        public string? vergiDairesiAdi { get; set; }
        public string? aciklama { get; set; }

        public string? profilFotografiUrl { get; set; }

        public IFormFile? profilFotografi { get; set; }

        public SelectList? DepartmentSel { get; set; }
        public string? departmanId { get; set; }

        public SelectList? PozisyonSel { get; set; }
        public string? pozisyonId { get; set; }
       
        public SelectList? SubeSel { get; set; }
        public string? subeId { get; set; }
        [RegularExpression(@"^[\d]+$", ErrorMessage = "Yıllık izin gün sayısı alanına sadece rakam girilebilir")]
        public string? yillikIzinGunSayisi { get; set; }
        
        [Range(0.0, 10.0, ErrorMessage = "Performans notu 0 ile 10 arasında bir değer olmalıdır.")]
        public string? performansNotu { get; set; }
        [Required(ErrorMessage = "SGK Sicil No alanı zorunludur.")]
        [RegularExpression(@"^[\d]+$", ErrorMessage = "SGK Sicil No alanına sadece rakam girilebilir")]
        public string? sgkSicilNo { get; set; }
        [Required(ErrorMessage = "Haftalık Saat alanı zorunludur.")]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "Haftalık saat değeri sadece rakamlardan oluşabilir ve, '12,30' , '12,3' veya '12'  formatında olabilir")]
        public string? haftalikSaat { get; set; }
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "Saatlik ücret değeri sadece rakamlardan oluşabilir ve, '12,30' , '12,3' veya '12'  formatında olabilir")]
        public string? saatlikUcret { get; set; }

        [Required(ErrorMessage = "Doğum tarihi alanı zorunludur.")]
        public DateTime dogumTarihi { get; set; }
        [Required(ErrorMessage = "Başlangıç tarihi alanı zorunludur.")]
        public DateTime baslangicTarihi { get; set; }
        public DateTime? bitisTarihi { get; set; }
        
        public bool? fazlaMesaiUygun { get; set; }

        [Required(ErrorMessage = "Çalışma tipi alanı zorunludur.")]
        public CalismaTipi CalismaTipi { get; set; } // Orijinal enum değeri
        [Required(ErrorMessage = "Cinsiyet alanı zorunludur.")]
        public Cinsiyet Cinsiyet { get; set; } // Orijinal enum değeri

        [Required(ErrorMessage = "Vardiya Türü alanı zorunludur.")]
        public VardiyaTuru VardiyaTuru { get; set; } // Orijinal enum değeri
        

    





    }
}
