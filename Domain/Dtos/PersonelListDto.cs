using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class PersonelListDto : Personel
    {
        public string? CalismaTipiStr { get; set; }

        public string departmentName { get; set; }
    }
}
