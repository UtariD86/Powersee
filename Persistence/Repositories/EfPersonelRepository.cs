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
        int? pageIndex = null,
        int? pageSize = null
        )


        {
            try { 
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
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                query = query.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            // Get paginated data
            var items = await query
                .Skip(Math.Max(0, (pageIndex.Value - 1) * pageSize.Value)) // Skip for pagination, ensure non-negative offset
                .Take(pageSize.Value) // Take the page size limit
                .ToListAsync();

            // Get the total count for pagination
            var totalCount = await query.CountAsync();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize.Value);


            var result = items.Select(personel => new PersonelListDto
            {
                Id = personel.Id,
                CalismaTipiStr = EnumHelper.ToSelectList<CalismaTipi>()
                        .FirstOrDefault(x => x.Value == personel.CalismaTipi.ToString())?.Text,



            }).ToList();

            // Return paginated response
            return new PageResponse<PersonelListDto>
            {
                Index = pageIndex.Value,
                Size = pageSize.Value,
                Count = totalCount,
                Pages = totalPages,
                HasPrevious = pageIndex > 1,
                HasNext = pageIndex < totalPages,
                Items = result
            };
            }catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}

