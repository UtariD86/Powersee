using Application.Helpers.Concrete;
using Core.Data.Repositories.Concrete;
using Core.Dtos.Concrete;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite.GeometriesGraph;
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
    public class EfTalepRepository :EfEntityRepositoryBase<Talep>, ITalepRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly ServiceProvider serviceProvider;

        public EfTalepRepository(ApplicationDbContext _context, IUnitOfWork unitOfWork) : base(_context)
        {
            context = _context;
            this.unitOfWork = unitOfWork;
        }


        public async Task<PageResponse<TalepDetailsDto>>
           GetAllTalepsAsync(
               Expression<Func<Talep, bool>>? predicate = null,
               Func<IQueryable<Talep>, IOrderedQueryable<Talep>>? orderBy = null,
               int pageIndex = 1,
               int pageSize = 10
           )
        {
            IQueryable<Talep> query = context.Set<Talep>();
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

            var result = items.Select(talep => new TalepDetailsDto
            {
                Id = talep.Id,

                gondericiId = talep.gondericiId,

                aliciId = talep.aliciId,

                aciklama = talep.aciklama,


                TalepTuru = talep.TalepTuru,
                TalepTuruStr = EnumHelper.GetDescription<TalepTuru>(talep.TalepTuru),

                Durum = talep.Durum,
                DurumStr = EnumHelper.GetDescription<Durum>(talep.Durum),

                planlanmisVardiyaId = talep.planlanmisVardiyaId,
                aliciName = context.Set<Personel>().FirstOrDefault(d => d.Id == talep.aliciId) is { } personel ? $"{personel.isim} {personel.soyisim}" : "Bir Hata Oluştu",


            }).ToList();

            // Get the total count for pagination
            var totalCount = await context.Set<Talep>().CountAsync();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Return paginated response
            return new PageResponse<TalepDetailsDto>
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
