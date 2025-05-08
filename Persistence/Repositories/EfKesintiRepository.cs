using Application.Helpers.Concrete;
using Core.Data.Repositories.Concrete;
using Core.Dtos.Concrete;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    public class EfKesintiRepository : EfEntityRepositoryBase<Kesinti>, IKesintiRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly ServiceProvider serviceProvider;

        public EfKesintiRepository(ApplicationDbContext _context, IUnitOfWork unitOfWork) : base(_context)
        {
            context = _context;
            this.unitOfWork = unitOfWork;
        }
        public async Task<PageResponse<KesintiListDto>>
          GetAllKesintisAsync(
              Expression<Func<Kesinti, bool>>? predicate = null,
              Func<IQueryable<Kesinti>, IOrderedQueryable<Kesinti>>? orderBy = null,
              int pageIndex = 1,
              int pageSize = 10
          )
        {
            IQueryable<Kesinti> query = context.Set<Kesinti>();
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

            var result = items.Select(Kesinti => new KesintiListDto
            {
                Id = Kesinti.Id,
                PersonelId=Kesinti.PersonelId,
                PlanlanmisVardiyaSnapshotId=Kesinti.PlanlanmisVardiyaSnapshotId,
                PlanlanmisName = context.Set<PlanlanmisVardiya>()
    .FirstOrDefault(d => d.Id == Kesinti.PlanlanmisVardiyaSnapshotId) is { } planlanmisVardiya ? $"Snapshot {planlanmisVardiya.Id} - {planlanmisVardiya.baslangicZamani:dd.MM.yyyy HH:mm}" : "Bir Hata Oluştu",
                PersonelName = context.Set<Personel>().FirstOrDefault(d => d.Id == Kesinti.PersonelId) is { } personel ? $"{personel.isim} {personel.soyisim}" : "Bir Hata Oluştu",
                CezaMiktari=Kesinti.CezaMiktari,
                UygulanacakTarih = Kesinti.UygulanacakTarih,
               

            }).ToList();

            // Get the total count for pagination
            var totalCount = await context.Set<Talep>().CountAsync();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Return paginated response
            return new PageResponse<KesintiListDto>
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