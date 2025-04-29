using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum UcretTuruEnum
    {
        [Display(Name = "Ücretli")]
        Ucretli = 1,

        [Display(Name = "Ücretsiz")]
        Ucretsiz,
    }
}