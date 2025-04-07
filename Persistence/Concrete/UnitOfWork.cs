using Persistence.Abstract;
using Persistence.Repositories;
using Persistence.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Concrete;

/// <summary>
/// Unit of Work tasarım deseni ile veritabanı işlemlerini yönetmek için kullanılan sınıf.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Veritabanı işlemlerini yönetmek için kullanılan context sınıfı.
    /// Context: Veritabanı bağlantısı, tablo oluşturma, veri ekleme, silme, güncelleme gibi işlemleri yapmamız için gerekli iletişimi sağlar.
    /// </summary>
    private readonly ApplicationDbContext _context;

    //Repository sınıflarını buraya ekleyebilirsiniz.
    private EfDepartmentRepository? _departmentRepository;

    private EfSubeRepository? _subeRepository;

    /// <param name="context"></param>

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    //repository sınıflarını buraya ekleyebilirsiniz.
    //Aşağıdaki işlem şu anlama gelir: Eğer _departmentRepository null ise yeni bir EfDepartmentRepository oluştur ve _departmentRepository'ye ata.
    //Eğer null değilse, zaten oluşturulmuş olan _departmentRepository'yi döndür.
    //ayrıca UnitOfwırk içinde kolayca erişebilemmiz için Departments adında bir property oluşturmuş olduk.
    public IDepartmentRepository Departments => _departmentRepository ?? new EfDepartmentRepository(_context, this);


    public ISubeRepository Subeler => _subeRepository ?? new EfSubeRepository(_context, this);

    /// <summary>
    /// Dispose: UnitOfWork sınıfı ile işimiz bittiğinde context nesnesini bellekten temizlemek için kullanılır.
    /// Çoğu zaman kullanılmasa da ard arda yapılan işlemlerde bellek sıkıntısı yaşanmaması için kullanılabilir.
    /// </summary>
    public void Dispose()
    {
        _context?.Dispose();
    }

    /// <summary>
    /// Veritabanı işlemlerini kaydetmek için kullanılır.
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Hata işleme kodlarını buraya ekleyin
            // ex hatası üzerinde çalışabilirsiniz veya loglama yapabilirsiniz
            throw; // İstisnayı yeniden fırlatmak veya alternatif bir işlem yapmak isterseniz throw ile fırlatabilirsiniz.
        }
    }
}