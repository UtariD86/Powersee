using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Admin.Models.Sube
{
    public class SubeDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        [MinLength(5, ErrorMessage = "İsim en az 5 karakter olmalıdır.")]
        [MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
        public string? Subeisim { get; set; }

        [Required(ErrorMessage = "Adres alanı zorunludur.")]
        [MinLength(5, ErrorMessage = "Adres en az 5 karakter olmalıdır.")]
        [MaxLength(50, ErrorMessage = "Adres en fazla 50 karakter olabilir.")]
        public string? Adres { get; set; }

        
        [MinLength(10, ErrorMessage = "Telefon numarası en az 10 karakter olmalıdır.")]
        [MaxLength(17, ErrorMessage = "Telefon numarası en fazla 16 karakter olabilir.")]
        [RegularExpression(@"^[\d +]+$", ErrorMessage = "Telefon numarası sadece sayı, boşluk ve + karakteri içerebilir.")]
        public string? TelefonNumarasi1 { get; set; }




    }
}
