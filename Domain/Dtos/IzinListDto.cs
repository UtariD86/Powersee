using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class IzinListDto : Izin
    {
        public string? IzinTuruStr { get; set; }

        public string? UcretTuruStr { get; set; }

        public string? FullName { get; set; }
    }
}