using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Admin.Models.Talep
{
    public class TalepDto
    {
        public int Id { get; set; }
        public int planlanmisVardiyaId { get; set; }
        
        public TalepTuru TalepTuru { get; set; }
        public SelectList? TalepTurusel { get; set; }
        public Durum Durum { get; set; } 
        public SelectList? Durumsel { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        [MinLength(5, ErrorMessage = "Açıklama en az 5 karakter olmalıdır.")]
        [MaxLength(50, ErrorMessage = "Açıklama en fazla 50 karakter olabilir.")]
        public string? aciklama { get; set; }
        public string gondericiId { get; set; }
        public int aliciId { get; set; }
        public SelectList? aliciSel { get; set; }
        
    }
}
