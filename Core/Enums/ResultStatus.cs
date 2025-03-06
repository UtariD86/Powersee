using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums;

/// <summary>
/// İşlem sonuçlarını standartize etmek için kullanılan enum tipi.
/// </summary>
public enum ResultStatus
{

    /// <summary>
    /// Hatalı sonuç
    /// </summary>
    Error = 0,

    /// <summary>
    /// Başarılı sonuç
    /// </summary>
    Success,

    /// <summary>
    /// Uyarı
    /// </summary>
    Warning,

    /// <summary>
    /// Bilgi
    /// </summary>
    Info
}
