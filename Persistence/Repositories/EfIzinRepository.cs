using Application.Helpers.Concrete;
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
    public class EfIzinRepository : EfEntityRepositoryBase<Izin>, IIzinRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly ServiceProvider serviceProvider;

        public EfIzinRepository(ApplicationDbContext _context, IUnitOfWork unitOfWork) : base(_context)
        {
            context = _context;
            this.unitOfWork = unitOfWork;
        }


        public async Task<PageResponse<IzinListDto>>
            GetAllIzinsAsync(
                Expression<Func<Izin, bool>>? predicate = null,
                Func<IQueryable<Izin>, IOrderedQueryable<Izin>>? orderBy = null,
                int pageIndex = 1,
                int pageSize = 10
            )
        {
            IQueryable<Izin> query = context.Set<Izin>();
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

            var result = items.Select(izin => new IzinListDto
            {
                Id = izin.Id,

                PersonelId = izin.PersonelId,

                BaslangicTarihi = izin.BaslangicTarihi,

                BitisTarihi = izin.BitisTarihi,

                IzinTuruEnum = izin.IzinTuruEnum,
                IzinTuruStr = EnumHelper.GetDescription<IzinTuruEnum>(izin.IzinTuruEnum),

                UcretTuruEnum = izin.UcretTuruEnum,
                UcretTuruStr = EnumHelper.GetDescription<UcretTuruEnum>(izin.UcretTuruEnum),

                Aciklama = izin.Aciklama,

                FullName = context.Set<Personel>().FirstOrDefault(d => d.Id == izin.PersonelId) is { } personel ? $"{personel.isim} {personel.soyisim}" : "Bir Hata Oluştu",


            }).ToList();

            // Get the total count for pagination
            var totalCount = await context.Set<Izin>().CountAsync();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Return paginated response
            return new PageResponse<IzinListDto>
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