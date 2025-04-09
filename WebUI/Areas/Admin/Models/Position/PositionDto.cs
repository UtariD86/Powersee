using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Admin.Models.Position
{
    public class PositionDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pozisyon adı zorunludur.")]
        [MinLength(5, ErrorMessage = "Pozisyon adı en az 5 karakter olmalıdır.")]
        [MaxLength(100, ErrorMessage = "Pozisyon adı en fazla 100 karakter olabilir.")]
        public string? Name { get; set; }

        public string? Code { get; set; } // Sistem tarafından atanacağı için validasyon yok

        /*  [Required(ErrorMessage = "Maaş alanı zorunludur.")]

          public decimal? Salary { get; set; }*/
        [Required(ErrorMessage = "Maaş alanı zorunludur.")]

        public string Salary{ get; set; }

        public int ManagerId { get; set; }
        public SelectList? ManagerList { get; set; }
       

        public bool? Active { get; set; }

    }
}
