using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum CalismaTipi
    {
        [Display(Name = "Tam Zamanlı")]
        TamZamanli =1,
        
        [Display(Name = "Yarı Zamanlı")]
        YariZamanli,

        [Display(Name = "Uzaktan")]
        Uzaktan
    }
}
