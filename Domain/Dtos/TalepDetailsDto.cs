using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class TalepDetailsDto :Talep
    {
        public string? TalepTuruStr { get; set; }

        public string? DurumStr { get; set; }
        public string? gondericiName { get; set; }
        public string? aliciName { get; set; }

        public string? planlanmisVardiyaName { get; set; }

    }
}
