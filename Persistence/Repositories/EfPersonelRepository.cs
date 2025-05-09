﻿using Application.Helpers.Concrete;
using Core.Data.Repositories.Concrete;
using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Core.Helpers.Abstract;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.GeometriesGraph;
using NetTopologySuite.Index.HPRtree;
using Persistence.Abstract;
using Persistence.Concrete;
using Persistence.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class EfPersonelRepository : EfEntityRepositoryBase<Personel>, IPersonelRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly ServiceProvider serviceProvider;

        public EfPersonelRepository(ApplicationDbContext _context, IUnitOfWork unitOfWork) : base(_context)
        {
            context = _context;
            this.unitOfWork = unitOfWork;
        }


        public async Task<PageResponse<PersonelListDto>>
            GetAllPersonelsAsync(
                Expression<Func<Personel, bool>>? predicate = null,
                Func<IQueryable<Personel>, IOrderedQueryable<Personel>>? orderBy = null,
                int pageIndex = 1,
                int pageSize = 10
            )
        {
                IQueryable<Personel> query = context.Set<Personel>();
                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                // Sayfalama (Paging)
                int skip = Math.Max(0, (pageIndex - 1) * pageSize);
                query = query.Skip(skip).Take(pageSize);


                // Get paginated data
                var items = await query.ToListAsync();

                var result = items.Select(personel => new PersonelListDto
                {
                    Id = personel.Id,
                    isim = personel.isim,
                    soyisim = personel.soyisim,
                    adres = personel.adres ?? "Bilgi Yok",
                    telefonNumarasi1 = personel.telefonNumarasi1,
                    telefonNumarasi2 = personel.telefonNumarasi2 ?? "Bilgi Yok",
                    tcKimlik = personel.tcKimlik,
                    bankaHesapNo = personel.bankaHesapNo ?? "Bilgi Yok",
                    vergiNo = personel.vergiNo ?? "Bilgi Yok",
                    vergiDairesiAdi = personel.vergiDairesiAdi ?? "Bilgi Yok",
                    aciklama = personel.aciklama ?? "Bilgi Yok",
                    departmanId = personel.departmanId,
                    pozisyonId = personel.pozisyonId,
                    subeId = personel.subeId,
                    yillikIzinGunSayisi = personel.yillikIzinGunSayisi,
                    performansNotu = personel.performansNotu,
                    sgkSicilNo = personel.sgkSicilNo,
                    haftalikSaat = personel.haftalikSaat,
                    saatlikUcret = personel.saatlikUcret ,
                    dogumTarihi = personel.dogumTarihi,
                    baslangicTarihi = personel.baslangicTarihi,
                    bitisTarihi = personel.bitisTarihi ?? DateTime.MinValue,
                    fazlaMesaiUygun = personel.fazlaMesaiUygun,
                    profilFotografiUrl = personel.profilFotografiUrl,
                    fazlaMesaiUygunStr = personel.fazlaMesaiUygun.HasValue ? (personel.fazlaMesaiUygun.Value ? "Uygun" : "Uygun Değil") : "Bilinmiyor",

                    CalismaTipi = personel.CalismaTipi,
                    CalismaTipiStr = EnumHelper.GetDescription<CalismaTipi>(personel.CalismaTipi),
                    
                    Cinsiyet = personel.Cinsiyet,
                    CinsiyetStr = EnumHelper.GetDescription<Cinsiyet>(personel.Cinsiyet),

                    VardiyaTuru = personel.VardiyaTuru,
                    VardiyaTuruStr = EnumHelper.GetDescription<VardiyaTuru>(personel.VardiyaTuru),

                    DepartmentSelName = (personel.departmanId.HasValue && context.Set<Department>().FirstOrDefault(d => d.Id == personel.departmanId) is { } department) ? department.Name : "Personel için departman girilmedi veya bulunamadı",

                    PositionSelName = (personel.pozisyonId.HasValue && context.Set<Domain.Entities.Position>().FirstOrDefault(p => p.Id == personel.pozisyonId) is { } position) ? position.Name : "Personel için pozisyon girilmedi veya bulunamadı",

                    SubeSelName = personel.subeId.HasValue && context.Set<Sube>().FirstOrDefault(s => s.Id == personel.subeId) is { } sube ? sube.Subeisim : "Personel için şube girilmedi veya bulunamadı",



                }).ToList();

                // Get the total count for pagination
                var totalCount = await context.Set<Personel>().CountAsync();

                // Calculate the total number of pages
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Return paginated response
                return new PageResponse<PersonelListDto>
                {
                    Index = pageIndex,
                    Size = pageSize,
                    Count = totalCount,
                    Pages = totalPages,
                    HasPrevious = pageIndex > 1,
                    HasNext = pageIndex < totalPages,
                    Items = result
                };
        }

    }
}