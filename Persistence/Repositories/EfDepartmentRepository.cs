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

        public async Task<PageResponse<DepartmentListDto>> GetDtoWithPaginationAsync(
      int pageIndex,
      int pageSize,
      Expression<Func<Department, bool>>? predicate = null,
      Func<IQueryable<Department>, IOrderedQueryable<Department>>? orderBy = null)
        {
            var dummyManagers = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "Ahmet Yılmaz" },
        new SelectListItem { Value = "2", Text = "Elif Demir" },
        new SelectListItem { Value = "3", Text = "Mehmet Kaya" }
    };

            // Önce filtrelenmiş ve sıralanmış departmentları al
            var baseQuery = context.Departments.AsQueryable();

            if (predicate != null)
                baseQuery = baseQuery.Where(predicate);

            if (orderBy != null)
                baseQuery = orderBy(baseQuery);

            var totalCount = await baseQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Dümdüz JOIN: Department x Position
            var joinedData = await (
                from dept in baseQuery
                join pos in context.Positions.Where(p => p.DeletedDate == null)
                    on dept.Id equals pos.DepartmentId into posGroup
                from pos in posGroup.DefaultIfEmpty()
                select new
                {
                    Department = dept,
                    Position = pos
                }
            )
            .ToListAsync();

            // Gruplama: Department başına bir grup
            var grouped = joinedData
                .GroupBy(x => x.Department)
                .Skip(Math.Max(0, (pageIndex - 1) * pageSize))
                .Take(pageSize)
                .Select(g => new DepartmentListDto
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    Description = g.Key.Description,
                    Adres = g.Key.Adres,
                    Managerid = g.Key.Managerid,
                    Active = g.Key.Active,
                    CreatedDate = g.Key.CreatedDate,
                    UpdatedDate = g.Key.UpdatedDate,
                    DeletedDate = g.Key.DeletedDate,
                    ManagerName = dummyManagers.FirstOrDefault(m => m.Value == g.Key.Managerid.ToString())?.Text ?? "Bilinmiyor",
                    ActiveStr = g.Key.Active.HasValue ? (g.Key.Active.Value ? "Aktif" : "Pasif") : "Bilinmiyor",
                    UniqueCode = g.Key.UniqueCode,
                    UniqueCodeStr = g.Key.UniqueCode,
                    CalismaTuruCal = g.Key.CalismaTuru,
                    Positions = g
                        .Where(x => x.Position != null)
                        .Select(p => new PositionDetailsDto
                        {
                            Id = p.Position.Id,
                            Name = p.Position.Name,
                            Active = p.Position.Active,
                            Salary = p.Position.Salary,
                            ManagerId=p.Position.ManagerId,
                            

                        }).ToList()
                })
                .ToList();

            return new PageResponse<DepartmentListDto>
            {
                Index = pageIndex,
                Size = pageSize,
                Count = totalCount,
                Pages = totalPages,
                HasPrevious = pageIndex > 1,
                HasNext = pageIndex < totalPages,
                Items = grouped
            };
        }

    }
}