using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Talep :EntityBase,IEntity 
    {
        public  string gondericiId { get; set; }
        public int aliciId { get; set; }
        public string aciklama { get; set; }
        public TalepTuru TalepTuru { get; set; }
        public Durum Durum { get; set; }
        public int planlanmisVardiyaId { get; set; }


    }
}
