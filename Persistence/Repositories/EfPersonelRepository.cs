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
                int pageIndex = 1,
                int pageSize = 10
            )
        {
            try
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
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

                // Get paginated data
                var items = await query.ToListAsync();

                var result = items.Select(personel => new PersonelListDto
                {
                    Id = personel.Id,
                    CalismaTipiStr = EnumHelper.ToSelectList<CalismaTipi>()
                        .FirstOrDefault(x => x.Value == personel.CalismaTipi.ToString())?.Text,
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}