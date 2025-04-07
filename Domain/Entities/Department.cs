using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Department : EntityBase, IEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string Adres { get; set; }




    public int Managerid { get; set; }
    public bool? Active { get; set; }

    public string? UniqueCode { get; set; }

    public CalismaTuru CalismaTuru { get; set; }


}


