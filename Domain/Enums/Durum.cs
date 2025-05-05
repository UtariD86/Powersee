using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum Durum
    {
        [Display(Name = "Beklemede")]
        Beklemede = 1,
        [Display(Name = "Reddedildi")]
        Reddedildi = 2,
        [Display(Name = "Kabul Edildi")]
        Kabul_Edildi = 3
    }
}