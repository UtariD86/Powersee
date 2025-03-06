using Core.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Core.Dtos.Abstract;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Core.Dtos.Concrete;

namespace Core.Data.Repositories.Concrete;
public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
{
    private readonly DbContext _context;
    //private readonly IHttpContextAccessor _httpContextAccessor;

    public EfEntityRepositoryBase(DbContext context/*, IHttpContextAccessor httpContextAccessor*/)
    {
        _context = context;
        //_httpContextAccessor = httpContextAccessor;
    }


    public void Add(TEntity Entity)
    {
        _context.Add(Entity);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task BulkInsert(IList<TEntity> entities)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var bulkConfig = new BulkConfig
            {
                PreserveInsertOrder = true,
                UseTempDB = true,
            };
            await _context.BulkInsertAsync(entities, bulkConfig);
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"Bir Hata oluştu: {ex.Message}");
        }
    }

    public async Task<IList<TEntity>> BulkInsertKeepIds(IList<TEntity> entities)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var bulkConfig = new BulkConfig
            {
                PreserveInsertOrder = true,
                SetOutputIdentity = true,
                UseTempDB = true,
            };
            await _context.BulkInsertAsync(entities, bulkConfig);
            transaction.Commit();
            return entities;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public void Delete(TEntity Entity)
    {
        _context.Entry(Entity).State = EntityState.Modified;
        _context.Set<TEntity>().Remove(Entity);
        _context.SaveChanges();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await Task.Run(() => { _context.Set<TEntity>().Remove(entity); });
    }

    public TEntity Get(int id)
    {
        return _context.Set<TEntity>().Find(id);
    }


    public async Task<IList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int? pageIndex = null,
        int? pageSize = null
        )
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();
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

        return await query.ToListAsync();
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (includeProperties.Any())
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.SingleOrDefaultAsync(); ;
    }

    public void Update(TEntity Entity)
    {
        _context.Attach(Entity);
        _context.Entry(Entity).State = EntityState.Modified;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        await Task.Run(() => { _context.Set<TEntity>().Update(entity); });
        return entity;
    }

    public TEntity UpdateWithStateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public async Task<PageResponse<TEntity>> GetEntitiesWithPaginationAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

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

        // Return paginated response
        return new PageResponse<TEntity>
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

}

