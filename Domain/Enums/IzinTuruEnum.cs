using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum IzinTuruEnum
    {
        [Display(Name = "Yıllık")]
        Yillik = 1,

        [Display(Name = "Mazeret")]
        Mazeret,

        [Display(Name = "Hastalık")]
        Hastalik,

        [Display(Name = "Doğum")]
        Dogum,

        [Display(Name = "Diğer")]
        Diger
    }
}