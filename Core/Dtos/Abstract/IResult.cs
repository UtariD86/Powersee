using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Abstract;

/// <summary>
/// İşlem sonuçlarını standartize etmek için kullanılan interface.
/// </summary>
public interface IResult
{
    /// <summary>
    /// İşlem sonucu enum değeri
    /// </summary>
    public ResultStatus ResultStatus { get; }//ResultStatus.Success // ResultStatus.Error gibi

    /// <summary>
    /// Sonuç mesajı
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Varsa Hata
    /// </summary>
    public Exception? Exception { get; }
}