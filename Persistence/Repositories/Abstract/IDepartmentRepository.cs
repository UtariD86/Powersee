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
    public interface IDepartmentRepository : IEntityRepository<Department>
    {

        Task<PageResponse<DepartmentListDto>> GetDtoWithPaginationAsync(
        int pageIndex,
        int pageSize,
        Expression<Func<Department, bool>>? predicate = null,
        Func<IQueryable<Department>, IOrderedQueryable<Department>>? orderBy = null);
    }
}
