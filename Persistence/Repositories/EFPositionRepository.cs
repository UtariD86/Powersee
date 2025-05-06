using Core.Data.Repositories.Concrete;
using Core.Dtos.Concrete;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public class EFPositionRepository : EfEntityRepositoryBase<Position>, IPositionRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly ServiceProvider serviceProvider;

        public EFPositionRepository(ApplicationDbContext _context, IUnitOfWork unitOfWork) : base(_context)
        {
            context = _context;
            this.unitOfWork = unitOfWork;
        }



    
     public async Task<PageResponse<PositionDetailsDto>> 
            GetDtoWithPaginationAsync(int pageIndex, int pageSize, Expression<Func<Position, bool>>? predicate = null, Func<IQueryable<Position>, IOrderedQueryable<Position>>? orderBy = null)
        {
            IQueryable<Position> query = context.Positions;

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

            // Join işlemi
            var joinedQuery = from pos in query
                              join dept in context.Departments on pos.DepartmentId equals dept.Id
                              join personel in context.Personels on pos.ManagerId equals personel.Id into personelGroup
                              from manager in personelGroup.DefaultIfEmpty()
                              where pos.DeletedDate == null && dept.DeletedDate == null
                              select new PositionDetailsDto
                              {
                                  Id = pos.Id,
                                  Name = pos.Name,
                                  DepartmentName = dept.Name ?? "Bilinmiyor",
                                  Code = pos.Code,
                                  Salary = pos.Salary,
                                  Active = pos.Active,
                                  PersonelName = manager != null ? $"{manager.isim} {manager.soyisim}" : "Bir Hata Oluştu"
                              };


            // Toplam kayıt sayısı
            var totalCount = await joinedQuery.CountAsync();

            // Sayfalama
            var items = await joinedQuery
                .Skip(Math.Max(0, (pageIndex - 1) * pageSize))
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return new PageResponse<PositionDetailsDto>
            {
                Index = pageIndex,
                Size = pageSize,
                Count = totalCount,
                Pages = totalPages,
                HasPrevious = pageIndex > 1,
                HasNext = pageIndex < totalPages,
                Items = items
            };
        }
    } }