using System;
using System.ComponentModel.DataAnnotations;
using Core.Enums; 
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Areas.Admin.Models.Izin
{
    public class IzinDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Personel ID alanı zorunludur.")]

        public int? PersonelId { get; set; }

        [BindNever]
        public SelectList? personelResultSel { get; set; }



        [MinLength(5, ErrorMessage = "Açıklama en az 5 karakter olmalıdır.")]
        [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Aciklama { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime BaslangicTarihi { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime BitisTarihi { get; set; }

        [BindNever]
        public SelectList? IzinTuruEnumSel { get; set; }

        [Required(ErrorMessage = "İzin türü zorunludur.")]
        [Display(Name = "İzin Türü")]
        public IzinTuruEnum IzinTuruEnum { get; set; }

        [BindNever]
        public SelectList? UcretTuruEnumSel { get; set; }

        [Required(ErrorMessage = "Ücret türü zorunludur.")]
        [Display(Name = "Ücret Türü")]
        public UcretTuruEnum UcretTuruEnum { get; set; }
    }
}



