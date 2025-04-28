using Core.Dtos.Abstract;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Concrete;

/// <summary>
/// İşlem sonuçlarını standartize etmek için kullanılan sınıf.
/// </summary>
public class Result : Core.Dtos.Abstract.IResult
{
    /// <summary>
    /// Sadece işlem sonucu döner.
    /// </summary>
    /// <param name="resultStatus"></param>
    public Result(ResultStatus resultStatus) => ResultStatus = resultStatus;

    /// <summary>
    /// İşlem sonucu ve mesaj döner.
    /// </summary>
    public Result(ResultStatus resultStatus, string message)
    {
        ResultStatus = resultStatus;
        Message = message;
    }

    /// <summary>
    /// İşlem sonucu ve hata döner.
    /// </summary>
    public Result(ResultStatus resultStatus, Exception exception)
    {
        ResultStatus = resultStatus;
        Exception = exception;
    }

    /// <summary>
    /// İşlem sonucu, mesaj ve hata döner.
    /// </summary>
    public Result(ResultStatus resultStatus, string message, Exception exception)
    {
        ResultStatus = resultStatus;
        Message = message;
        Exception = exception;
    }

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