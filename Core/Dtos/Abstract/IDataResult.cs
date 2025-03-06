using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Abstract;

/// <summary>
/// Veri döndüren işlemler için kullanılacak olan interface.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDataResult<out T> : IResult
{
    /// <summary>
    /// Veri
    /// </summary>
    public T Data { get; }//new DataResult<Category>(ResultStatus.Success,category)
                          //new DataResult<IList<Category>>(ResultStatus.Success,categoryList)
}
