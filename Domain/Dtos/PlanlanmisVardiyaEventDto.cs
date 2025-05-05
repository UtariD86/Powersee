using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class PlanlanmisVardiyaEventDto
    {
        public string id { get; set; }
        public string title { get; set; }
        public string start { get; set; }  // ISO 8601 string formatı
        public string? end { get; set; }
        public string? description { get; set; }
        public string? className { get; set; }
    }
}
