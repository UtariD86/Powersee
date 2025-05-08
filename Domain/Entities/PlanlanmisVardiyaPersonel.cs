using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PlanlanmisVardiyaPersonel : EntityBase, IEntity
    {
        public int PersonelId { get; set; }

        public int PlanlanmisVardiyaId { get; set; }

        public DateTime? girisZamani { get; set; }
        public DateTime? cikisZamani { get; set; }
        public decimal? hedefUcret { get; set; }
        public decimal? kazanilanUcret { get; set; }
    }
}
