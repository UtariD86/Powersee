using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Vardiya : EntityBase, IEntity
{
    public string vardiyaIsmi { get; set; }
    public TimeOnly baslangicSaati { get; set; }
    public TimeSpan calismaSuresi { get; set; }
    public string? aciklama { get; set; }
    public bool? listelenecek { get; set; }
    public decimal ucretKatsayisi{ get; set; }
    public TimeSpan esneklikPayiSuresi { get; set; }

}
