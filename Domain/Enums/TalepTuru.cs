using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum TalepTuru
    {
        [Display(Name = "İzin Talebi")]
        Izin_Talebi= 1,

        [Display(Name = "Değişim Talebi")]
        Degisim_Talebi=2,

        [Display(Name = "Şikayet")]
        Sikayet=3,

        [Display(Name = "İhbar")]
        Ihbar=4
    }
}

