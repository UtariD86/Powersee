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
    public interface IPersonelRepository : IEntityRepository<Personel>
    {

        public Task<PageResponse<PersonelListDto>>

            GetAllPersonelsAsync(
        Expression<Func<Personel, bool>>? predicate = null,
        Func<IQueryable<Personel>, IOrderedQueryable<Personel>>? orderBy = null,
        int pageIndex = 1,
        int pageSize = 10
        );
    }
}
