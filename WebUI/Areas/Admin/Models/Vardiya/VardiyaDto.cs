using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Admin.Models.Vardiya
{
    public class VardiyaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vardiya İsmi alanı zorunludur.")]
        public string vardiyaIsmi { get; set; }

        [Required(ErrorMessage = "Başlangıç Saati alanı zorunludur.")]
        public TimeOnly baslangicSaati { get; set; }

        [Required(ErrorMessage = "Çalışma Süresi alanı zorunludur.")]
        public TimeSpan calismaSuresi { get; set; }
        
        [MaxLength(200, ErrorMessage = "Açıklama alanı en fazla 200 karakter olabilir.")]
        public string? aciklama { get; set; }

        public bool? listelenecek { get; set; }

        [Required(ErrorMessage = "Ücret Katsayısı alanı zorunludur.")]
        public string ucretKatsayisi { get; set; }

        [Required(ErrorMessage = "Esneklik Payı Süresi alanı zorunludur.")]
        public TimeSpan esneklikPayiSuresi { get; set; }




    }
}
