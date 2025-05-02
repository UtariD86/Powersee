using Microsoft.AspNetCore.Mvc.Rendering; // SelectListItem için eklendi
using System; // DateTime için eklendi
using System.Collections.Generic; // IEnumerable için eklendi
using System.ComponentModel.DataAnnotations; // Validation attribute'ları için

namespace WebUI.Areas.Admin.Models.Kesinti // Namespace Kesinti olarak güncellendi
{
    /// <summary>
    /// Kesinti ekleme/düzenleme işlemleri için kullanılan Veri Transfer Nesnesi (DTO).
    /// </summary>
    public class KesintiDto
    {
        /// <summary>
        /// Kesintinin benzersiz kimliği (Düzenleme için kullanılır).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Kesintinin uygulanacağı personelin ID'si.
        /// </summary>
        [Required(ErrorMessage = "Personel seçimi zorunludur.")] // Zorunlu alan
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir personel seçiniz.")] // Seçimin yapıldığından emin olmak için
        public int PersonelId { get; set; }

        /// <summary>
        /// Kesintinin ilişkili olduğu planlanmış vardiya snapshot'ının ID'si.
        /// </summary>
        //[Required(ErrorMessage = "Vardiya Snapshot seçimi zorunludur.")] // Zorunlu alan
        //[Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir vardiya snapshot seçiniz.")] // Seçimin yapıldığından emin olmak için
        public int? PlanlanmisVardiyaSnapshotId { get; set; }

        /// <summary>
        /// Kesintinin uygulanacağı tarih.
        /// </summary>
        [Required(ErrorMessage = "Uygulanacak tarih alanı zorunludur.")] // Zorunlu alan
        [DataType(DataType.Date)] // Veri tipini belirtiyoruz
        public DateTime UygulanacakTarih { get; set; }

        /// <summary>
        /// Uygulanacak ceza miktarı (tutar).
        /// </summary>
        [Required(ErrorMessage = "Ceza miktarı alanı zorunludur.")] // Zorunlu alan
        [Range(0.01, double.MaxValue, ErrorMessage = "Ceza miktarı 0'dan büyük olmalıdır.")] // Pozitif bir değer olmalı
        [DataType(DataType.Currency)] // Para birimi formatı için ipucu
        public decimal CezaMiktari { get; set; }

        // --- Dropdown Listeleri İçin Özellikler ---
        // Bu özellikler veritabanına kaydedilmez, sadece view'da listeyi göstermek için kullanılır.
        // Controller tarafında doldurulmalıdırlar.

        /// <summary>
        /// Personel dropdown'ını doldurmak için kullanılacak liste.
        /// </summary>
        public IEnumerable<SelectListItem>? PersonelListesi { get; set; }

        /// <summary>
        /// Vardiya Snapshot dropdown'ını doldurmak için kullanılacak liste.
        /// </summary>
        public IEnumerable<SelectListItem>? SnapshotListesi { get; set; }

        // --- Constructor (Dropdown listelerini başlatmak için) ---
        public KesintiDto()
        {
            PersonelListesi = new List<SelectListItem>();
            SnapshotListesi = new List<SelectListItem>();
            UygulanacakTarih = DateTime.Today; // Varsayılan olarak bugünün tarihini atayabiliriz
        }
    }
}
