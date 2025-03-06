namespace Core.Dtos.Concrete;

/// <summary>
/// Tüm entitylerde ortak olan alanlar için kullanılacak base class.
/// </summary>
public class EntityBase
{
    /// <summary>
    /// Entity'nin Id'si.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Entity'nin oluşturan kullanıcısının Id'si.
    /// </summary>
    //public int CreatedById { get; set; }

    /// <summary>
    /// Oluşturma tarihi.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Güncellenme tarihi. (Ayrıca tutmamızın sebebi daha sonra sıralama yaparken kullanılabilir.)
    /// </summary>
    public DateTime UpdatedDate { get; set; }

    /// <summary>
    /// Silinme tarihi. (null değilse silinmiş demektir.)
    /// </summary>
    public DateTime? DeletedDate { get; set; }
}

