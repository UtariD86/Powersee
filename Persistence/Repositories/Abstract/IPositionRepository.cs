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
    public interface IPositionRepository : IEntityRepository<Position>
    {

        public  Task<PageResponse<PositionDetailsDto>>
               GetDtoWithPaginationAsync(int pageIndex, int pageSize, Expression<Func<Position, bool>>? predicate = null, Func<IQueryable<Position>, IOrderedQueryable<Position>>? orderBy = null);



    }
}
