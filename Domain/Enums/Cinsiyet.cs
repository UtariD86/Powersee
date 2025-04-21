using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum Cinsiyet
    {

        [Display(Name = "Erkek")]
        Erkek = 1,
        
        [Display(Name = "Kadın")]
        Kadin,

        [Display(Name = "Diğer")]
        Diger
    }
}
