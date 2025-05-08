using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Admin.Models.PlanlanmisVardiya
{
    public class PlanlanmisVardiyaDto
    {
        public int Id { get; set; }


       

        public SelectList? personelIdSel { get; set; }

        //[Required(ErrorMessage = "Personel alanı zorunludur.")]
        public List<int>? personelIds { get; set; }
        public string? personelIdsStr { get; set; }
        
        public List<int>? Personels { get; set; }
        public SelectList? vardiyaIdSel { get; set; }

        [Required(ErrorMessage = "vardiyaId alanı zorunludur.")]
        public int? vardiyaId { get; set; }

        [Required(ErrorMessage = "baslangicZamani alanı zorunludur.")]
        public DateTime? baslangicZamani { get; set; }

        [Required(ErrorMessage = "bitisZamani alanı zorunludur.")]
        public DateTime? bitisZamani { get; set; }

        //public DateTime? girisZamani { get; set; }

        //public DateTime? cikisZamani { get; set; }

        //[Range(0, double.MaxValue, ErrorMessage = "hedefUcret pozitif bir değer olmalıdır.")]
        //public decimal? hedefUcret { get; set; }

        //[Range(0, double.MaxValue, ErrorMessage = "kazanilanUcret pozitif bir değer olmalıdır.")]
        //public decimal? kazanilanUcret { get; set; }



    }
}
