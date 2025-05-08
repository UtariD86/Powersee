using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Personel : EntityBase, IEntity
{
    public string isim { get; set; }
    public string soyisim { get; set; }
    public string? adres { get; set; }
    public string telefonNumarasi1 { get; set; }
    public string? telefonNumarasi2 { get; set; }
    public string tcKimlik { get; set; }
    public string? bankaHesapNo { get; set; }
    public string? vergiNo { get; set; }
    public string? vergiDairesiAdi { get; set; }
    public string? aciklama { get; set; }

    public string Code { get; set; }

    [NotMapped]
    public IFormFile? profilFotografi { get; set; }
    public string? profilFotografiUrl { get; set; }

    

    public int? departmanId { get; set; }
    public int? pozisyonId { get; set; }
    public int? subeId { get; set; }
    public int? yillikIzinGunSayisi { get; set; }
    public int? performansNotu { get; set; }
    public int sgkSicilNo { get; set; }
    
    
    public decimal haftalikSaat { get; set; }
    public decimal? saatlikUcret { get; set; }


    public DateTime dogumTarihi { get; set; }
    public DateTime baslangicTarihi { get; set; }
    public DateTime? bitisTarihi { get; set; }


    public bool? fazlaMesaiUygun { get; set; }


    public CalismaTipi CalismaTipi { get; set; }
    public Cinsiyet Cinsiyet { get; set; }
    public VardiyaTuru VardiyaTuru { get; set; }

}
