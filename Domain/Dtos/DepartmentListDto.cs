using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class DepartmentListDto : Department
    {
        public string ManagerName { get; set; }
        public string ActiveStr { get; set; }
        public string UniqueCode { get; set; }

        // Get-Set ile güncellenmiş hali:
        public string UniqueCodeStr
        {
            get => UniqueCode;
            set => UniqueCode = value;
        }

        public CalismaTuru CalismaTuruCal { get; set; } // Enum alan
        public string CalismaTuruStr => CalismaTuruCal.ToString(); // Enum string dönüşü
    }
}