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
    public interface IKesintiRepository : IEntityRepository<Kesinti>
    {
        Task<PageResponse<KesintiListDto>>
          GetAllKesintisAsync(
              Expression<Func<Kesinti, bool>>? predicate = null,
              Func<IQueryable<Kesinti>, IOrderedQueryable<Kesinti>>? orderBy = null,
              int pageIndex = 1,
              int pageSize = 10
          );
    }
}