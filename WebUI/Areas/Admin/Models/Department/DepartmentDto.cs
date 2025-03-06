using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Admin.Models.Department
{
    public class DepartmentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        [MinLength(5, ErrorMessage = "İsim en az 5 karakter olmalıdır.")]
        [MaxLength(50, ErrorMessage = "İsim en fazla 50 karakter olabilir.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        [MinLength(5, ErrorMessage = "Açıklama en az 5 karakter olmalıdır.")]
        [MaxLength(50, ErrorMessage = "Açıklama en fazla 50 karakter olabilir.")]
        public string? Description { get; set; }
    }
}
