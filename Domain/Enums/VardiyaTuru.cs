using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum VardiyaTuru
    {

        [Display(Name = "Gündüz")]
        Gunduz = 1,

        [Display(Name = "Gece")]
        Gece
        
    }
}