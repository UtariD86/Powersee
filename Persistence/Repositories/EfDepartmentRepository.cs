using Core.Data.Repositories.Concrete;
using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class EfDepartmentRepository : EfEntityRepositoryBase<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly ServiceProvider serviceProvider;

        public EfDepartmentRepository(ApplicationDbContext _context, IUnitOfWork unitOfWork) : base(_context)
        {
            context = _context;
            this.unitOfWork = unitOfWork;
        }

        public async Task<PageResponse<DepartmentListDto>> GetDtoWithPaginationAsync(int pageIndex, int pageSize, Expression<Func<Department, bool>>? predicate = null, Func<IQueryable<Department>, IOrderedQueryable<Department>>? orderBy = null)
        {
            IQueryable<Department> query = context.Set<Department>();

            // Apply filter if any
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // Apply ordering if provided
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Get paginated data
            var items = await query
                .Skip(Math.Max(0, (pageIndex - 1) * pageSize)) // Skip for pagination, ensure non-negative offset
                .Take(pageSize) // Take the page size limit
                .ToListAsync();

            // Get the total count for pagination
            var totalCount = await query.CountAsync();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var dummyManagers = new List<SelectListItem>
{
    new SelectListItem { Value = "1", Text = "Ahmet Yılmaz" },
    new SelectListItem { Value = "2", Text = "Elif Demir" },
    new SelectListItem { Value = "3", Text = "Mehmet Kaya" }
};

            var result = items.Select(dept => new DepartmentListDto
            {
                Id = dept.Id,
                Name = dept.Name,
                Description = dept.Description,
                Adres = dept.Adres,
                Managerid = dept.Managerid,
                Active = dept.Active,
                CreatedDate = dept.CreatedDate,
                UpdatedDate = dept.UpdatedDate,
                DeletedDate = dept.DeletedDate,
                ManagerName = dummyManagers
                                .FirstOrDefault(m => m.Value == dept.Managerid.ToString())?.Text ?? "Bilinmiyor",
                ActiveStr = dept.Active.HasValue ? (dept.Active.Value ? "Aktif" : "Pasif") : "Bilinmiyor",

                UniqueCode = dept.UniqueCode,
                UniqueCodeStr = dept.UniqueCode,

                CalismaTuruCal = dept.CalismaTuru,

            }).ToList();

            // Return paginated response
            return new PageResponse<DepartmentListDto>
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
