using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class PlanlanmisVardiya : EntityBase, IEntity
{
    public int personelId { get; set; }
    public int vardiyaId { get; set; }
    public DateTime baslangicZamani { get; set; }
    public DateTime bitisZamani { get; set; }
    public DateTime? girisZamani { get; set; }
    public DateTime? cikisZamani { get; set; }
    public decimal? hedefUcret { get; set; }
    public decimal? kazanilanUcret { get; set; }


}
