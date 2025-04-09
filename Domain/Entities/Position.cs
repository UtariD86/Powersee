using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class Position :EntityBase,IEntity
    {
      
        public string Name { get; set; }

      
        public string Code { get; set; } // 5 harf + 5 rastgele karakter


        public decimal Salary { get; set; } // Tipini decimal olarak değiştirin

        public int ManagerId { get; set; }
       

        public bool? Active { get; set; }

    }
}
