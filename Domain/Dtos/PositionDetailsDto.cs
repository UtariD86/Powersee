using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Dtos
{
    public class PositionDetailsDto :Position 
    {
        public string DepartmentName { get; set; }
    }
}
