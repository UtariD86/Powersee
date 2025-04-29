using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Izin : EntityBase, IEntity
    {

        public int Id { get; set; }


        public int PersonelId { get; set; }


        public DateTime BaslangicTarihi { get; set; }


        public DateTime BitisTarihi { get; set; }


        public IzinTuruEnum IzinTuruEnum { get; set; }

        public UcretTuruEnum UcretTuruEnum { get; set; }


        public string? Aciklama { get; set; }
    }
}