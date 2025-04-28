using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum CalismaTuru
    {
        [Display(Name = "Hibrit")]
        Hibrit,
        [Display(Name = "Uzaktan")]
        Uzaktan,
        [Display(Name = "İşyerinden")]
        Isyerinden
    }
}
