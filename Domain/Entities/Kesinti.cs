using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Kesinti : EntityBase, IEntity
{
    public int PersonelId { get; set; } 
    public int PlanlanmisVardiyaSnapshotId { get; set; }
    public DateTime UygulanacakTarih { get; set; }
    public decimal CezaMiktari { get; set; }
}