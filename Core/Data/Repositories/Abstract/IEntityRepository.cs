﻿using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract;

/// <summary>
/// Tüm entity repositorylerin implement etmesi gereken interface. Repository yazarken bu interface miras alınarak standart işlemler yapılabilir.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEntityRepository<T> where T : class, IEntity, new()
{
    T Get(int id);
    /// <summary>
    /// Kayıt ekle.
    /// </summary>
    /// <param name="Entity"></param>
    void Add(T Entity);

    /// <summary>
    /// Kayıt Güncelle.
    /// </summary>
    /// <param name="Entity"></param>
    void Update(T Entity);

    /// <summary>
    /// entity ile kayıt sil.
    /// </summary>
    /// <param name="Entity"></param>
    void Delete(T Entity);


    #region dynamic Repository
    /// <summary>
    /// Asenkron olarak ilgili kaydı verilen predicateye göre getirir.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includeProperties"></param>
    /// <returns></returns>
    Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

    //var kullanici = repository.GetAsync(k=>k.Id==15)
    //IQueryable<T> GetAsQueryable();
    /// <summary>
    /// Asenkron olarak tüm kayıtları IList döner, predicate verilebilir.
    /// </summary>
    Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? pageIndex = null,
        int? pageSize = null);

    /// <summary>
    /// Asenkron olarak kayıt ekler.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Asenkron olarak kayıt günceller.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// denemek için yazıldı. state vererek güncelleme
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    T UpdateWithStateAsync(T entity);

    /// <summary>
    /// Asenkron olarak kayıt siler. (Entity ile)
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Verilen predicate'deki kayıt var mı yok mu?
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Verilen predicate'de kaç tane kayıt var?
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Asenkron olarak topluca ekleme işlemi yapar. Eğer başarısız bir ekleme adımı olursa hepsini geri alır
    /// </summary>
    /// <param name="entities"> Topluca eklenecek entity listesi</param>
    /// <returns></returns>
    Task BulkInsert(IList<T> entities);

    /// <summary>
    /// Asenkron olarak topluca ekleme işlemi yapar. Idleri tutar. Eğer başarısız bir ekleme adımı olursa hepsini geri alır
    /// </summary>
    /// <param name="entities"> Topluca eklenecek entity listesi</param>
    /// <returns></returns>
    Task<IList<T>> BulkInsertKeepIds(IList<T> entities);

    /// <summary>
    /// Sayfalama yaparak veri getirir.
    /// </summary>
    Task<PageResponse<T>> GetEntitiesWithPaginationAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    #endregion
}
