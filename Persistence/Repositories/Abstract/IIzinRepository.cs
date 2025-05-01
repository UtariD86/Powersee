using Core.Data.Repositories.Abstract;
using Core.Dtos.Concrete;
using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.Abstract
{
    public interface IIzinRepository : IEntityRepository<Izin>
    {

        public Task<PageResponse<IzinListDto>>

            GetAllIzinsAsync(
        Expression<Func<Izin, bool>>? predicate = null,
        Func<IQueryable<Izin>, IOrderedQueryable<Izin>>? orderBy = null,
        int pageIndex = 1,
        int pageSize = 10
        );
    }
}