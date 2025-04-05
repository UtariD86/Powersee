using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Sube : EntityBase, IEntity
{
    public string Subeisim { get; set; }
    public string Adres { get; set; }
    public string TelefonNumarasi1 { get; set; }
}
