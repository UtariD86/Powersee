using Core.Dtos.Abstract;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Concrete;

/// <summary>
/// Veri ve Durum dönerken kullanılır.
/// </summary>
/// <typeparam name="T">Entity tipi</typeparam>
public class DataResult<T> : IDataResult<T>
{
    /// <summary>
    /// Veri ve Durum dönerken kullanılır.
    /// </summary>
    public DataResult(ResultStatus resultStatus, T data)
    {
        ResultStatus = resultStatus;
        Data = data;
    }

    /// <summary>
    /// Veri, Durum ve Mesaj dönerken kullanılır.
    /// </summary>
    public DataResult(ResultStatus resultStatus, string message, T data)
    {
        ResultStatus = resultStatus;
        Message = message;
        Data = data;
    }

    /// <summary>
    /// Veri, Durum, Mesaj ve Hata dönerken kullanılır.
    /// </summary>
    public DataResult(ResultStatus resultStatus, string message, T data, Exception exception)
    {
        ResultStatus = resultStatus;
        Message = message;
        Data = data;
        Exception = exception;
    }

    /// <summary>
    /// Veri
    /// </summary>
    public T Data { get; }

    /// <summary>
    /// İşlem sonucu enum değeri
    /// </summary>
    public ResultStatus ResultStatus { get; }

    /// <summary>
    /// Sonuç mesajı
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Varsa Hata
    /// </summary>
    public Exception Exception { get; }
}