using Persistence.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Abstract;

/// <summary>
/// Unit of Work tasarım deseni ile veritabanı işlemlerini yönetmek için kullanılan interface.
/// Bu tasarım deseni ile veritabanı işlemleri tek bir noktadan yönetilir.
/// Örneğin, bir işlem sırasında birden fazla tabloya veri eklememiz gerektiğinde, 
/// bu işlemleri tek bir SaveChangesAsync() metodu ile yönetebiliriz.
/// Veya, Application katmanından veritabanı işlemlerini daha temiz bir şekilde yönetebiliriz.
/// Örneğin, bir Controller sınıfında birden fazla Repository sınıfı kullanmak yerine,
/// yalnızca IUnitOfWork interface'ini kullanarak veritabanı işlemlerini yönetebiliriz.
/// _unitOfWork.EntityAdi.AddAsync(entity) şeklinde tek bir noktadan veritabanı işlemlerini yönetebiliriz.
/// </summary>
public interface IUnitOfWork
{

    //Burada pattern içersinde kullanılacak olan Repository sınıflarını belirliyoruz ayrıca kolay erişebilmek için 
    //Property isimlendirmeleri yapıyoruz.
    //IExampleRepository Example { get; }
    IDepartmentRepository Departments { get; }

    IIzinRepository Izinler { get; }
    IKesintiRepository Kesintiler { get; }

    IPositionRepository Positions { get; }

    ISubeRepository Subeler { get; }

    IPersonelRepository Personels { get; }
    /// <summary>
    /// Veritabanı işlemlerini kaydetmek için kullanılır.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();
}