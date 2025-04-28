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

        public string? CinsiyetStr { get; set; }

        public string? VardiyaTuruStr { get; set; }

        public string fazlaMesaiUygunStr { get; set; }

        public string DepartmentSelName { get; set; }

        public string PositionSelName { get; set; }

        public string SubeSelName { get; set; }
    }
}
