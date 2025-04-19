using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Areas.Admin.Models.Personel
{





    public class PersonelDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        [MinLength(5, ErrorMessage = "İsim en az 5 karakter olmalıdır.")]
        [MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
        public string? isim { get; set; }
        public string? soyisim { get; set; }
        public string? adres { get; set; }
        public string? telefonNumarasi1 { get; set; }
        public string? telefonNumarasi2 { get; set; }
        public string? tcKimlik { get; set; }
        public string? bankaHesapNo { get; set; }
        public string? vergiNo { get; set; }
        public string? vergiDairesiAdi { get; set; }
        public string? aciklama { get; set; }


        public SelectList DepartmentSel { get; set; }
        public string? departmanId { get; set; }

        public SelectList PozisyonSel { get; set; }
        public string? pozisyonId { get; set; }
        public SelectList SubeSel { get; set; }


        public string? subeId { get; set; }
        public string? yillikIzinGunSayisi { get; set; }
        public string? performansNotu { get; set; }
        public string? sgkSicilNo { get; set; }

        public string? haftalikSaat { get; set; }
        public string? saatlikUcret { get; set; }


        public string? dogumTarihi { get; set; }
        public string? baslangicTarihi { get; set; }
        public string? bitisTarihi { get; set; }
        
        public bool fazlaMesaiUygun { get; set; }


        public CalismaTipi CalismaTipi { get; set; } // Orijinal enum değeri
        
        public Cinsiyet Cinsiyet { get; set; } // Orijinal enum değeri
        

        public VardiyaTuru VardiyaTuru { get; set; } // Orijinal enum değeri
        

    





    }
}
