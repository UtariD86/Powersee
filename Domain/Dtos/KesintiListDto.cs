using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class KesintiListDto :Kesinti
    {
        public string PersonelName { get; set; }
        public string PlanlanmisName { get; set; }
    }
}
