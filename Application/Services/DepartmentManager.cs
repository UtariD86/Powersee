using Core.Helpers.Abstract;
using Application.Helpers.Concrete;
using Application.Helpers.Concrete.Filtering;
using Application.Services.Abstract;
using Azure.Core;
using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using Domain.Dtos;
using Domain.Entities;
using Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    //Bu sınıf IDepartmentService interfaceinden türetilmiştir ve IDepartmentService arayüzündeki metotları uygulamaktadır.
    //IDepartmentService interfaceinde tanımladığımız metotlara burada işlevsellik kazandırıyoruz.
    public class DepartmentManager : IDepartmentService
    {
        //Repositorylere erişim sağlamak için IUnitOfWork kullanıyoruz.
        private readonly IUnitOfWork _unitOfWork;

        //Burada filtreleme için özel yazılmış bir yardımcı sınıfı kullanıyoruz.
        private readonly FilterHelper _filterHelper;

        //private readonly IEnumHelper _enumHelper;

        //Dependency Injection kullanarak IUnitOfWork tipinde bir parametre alıyoruz.
        public DepartmentManager(IUnitOfWork unitOfWork, FilterHelper filterHelper /*IEnumHelper enumHelper*/)
        {
            //Dependency Injection ile gelen parametreyi _unitOfWork ve _filterHelper değişkenine atıyoruz.
            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;

            //_enumHelper = enumHelper;

            //Böylece IOC container bize ihtiyaç halinde IUnitOfWork tipinde bir nesne sağlayacak.
            //Bu işlem bizim newleme işlemlerimizi minimize edecek ve bağımlılıklarımızı minimize edecek.
            //Çok daha esnek bir yapıda çalışmış olacağız.
            //Daha az kaynak tüketimi ve daha az bellek kullanımı ile uygulamamız daha performanslı çalışacak.
        }

        //Bu metot bizim ekleme ve güncelleme işlemlerimizi yapacak.
        public async Task<IDataResult<Department>> Edit(Department department)
        {
            //Hata yakalarak sonuç dönebiliyoruz.
            try
            {
                //Eğer gelen departman nesnesi null değilse ve departman nesnesinin Id'si 0'dan farklı ise
                //Bu işlem varolan bir nesne için kullanılacağı anlamına gelir. Yani günceleme işlemi yapılacak demektir.
                if (department != null && department.Id != 0)
                {
                    department.UpdatedDate = DateTime.UtcNow;
                    //Buraya gerekirse kontroller validasyon mesajları vb ekleyebiliriz.

                    //Güncelleme İşlemi
                    await _unitOfWork.Departments.UpdateAsync(department);

                    //SaveChangesAsync metodu yazılana kadar yapılacak veritabanı işlemleri birikir.
                    //SaveChangesAsync metodu ile bu işlemler veritabanına kaydedilir.
                    await _unitOfWork.SaveChangesAsync();

                    //Başarılı bir şekilde güncelleme işlemi yapıldıysa aşağıdaki mesajı döndürürüz.
                    return new DataResult<Department>(
                        data: department,
                        resultStatus: ResultStatus.Success,
                        message: $"{department.Name} isimli departman başarıyla güncellendi."
                        );
                }
                else //Eğer gelen departman nesnesi null ise veya departman nesnesinin Id'si 0 ise bu bir ekleme işlemidir.
                {
                    department.UpdatedDate = DateTime.UtcNow;
                    department.CreatedDate = DateTime.UtcNow;
                    //Ekleme İşlemi

                    department.UniqueCode = KodOlustur(department.Name);


                    //Departmanın ismi ile aynı isme sahip bir departman var mı diye kontrol ediyoruz.
                    //Eşsiz olmasını istediğimiz için yalnızca bir kontrol. Şart değil.
                    var checkDepartment = await _unitOfWork.Departments.GetAsync(d => d.Name == department.Name && department.DeletedDate == null);

                    //Eğer şartımıza uymazsa bu şekilde bir hata dönebiliriz.
                    if (checkDepartment != null)
                    {
                        return new DataResult<Department>(
                            resultStatus: ResultStatus.Error,
                            message: "Departman Halihazırda Mevcut",
                            data: null);
                    }

                    //Eğer şartımıza uyar ve yeni bir departman eklenmek isteniyorsa ekleme işlemini çağırıyoruz.
                    await _unitOfWork.Departments.AddAsync(department);

                    //SaveChangesAsync metodu yazılana kadar yapılacak veritabanı işlemleri birikir.
                    //SaveChangesAsync metodu ile bu işlemler veritabanına kaydedilir.
                    await _unitOfWork.SaveChangesAsync();

                    //Başarılı bir şekilde ekleme işlemi yapıldıysa aşağıdaki mesajı döndürürüz.
                    return new DataResult<Department>(ResultStatus.Success, department.Name + "Başarıyla Eklendi", department);
                }
            }
            catch (Exception ex)
            {
                //Eğer try bloğu içerisinde bir hata oluşursa bu hatayı döneriz.
                return new DataResult<Department>(
                   resultStatus: ResultStatus.Error,
                   message: $"Malesef bir sorun oluştu...",
                   exception: ex,
                   data: null
                   );
            }
        }



        //Bu metot silme işlemlerini yapacak.
        public async Task<IResult> Delete(int Id)
        {
            //öncelikle gönderilen departman Idsinin veritabanında olup olmadığını kontrol ediyoruz.
            var _department = _unitOfWork.Departments.Get(Id);

            //Eğer departman null değilse
            if (_department != null)
            {
                //Departmanı silmek için DeletedDate alanına şu anki tarihi atıyoruz.
                //Yani gerçekten silmek yerine kullanıcının göremeyeceği bir şekilde işaretliyoruz.
                //Bu işleme soft delete denir.
                _department.DeletedDate = DateTime.UtcNow;

                //Ayrıca güncelleme tarihini de güncelliyoruz ki Eğer silinen öğeleri görüntülemek istersek işlem tarhine göre yapabilelim.
                _department.UpdatedDate = DateTime.UtcNow;

                //Departmanı güncelliyoruz.
                await _unitOfWork.Departments.UpdateAsync(_department);

                //SaveChangesAsync metodu yazılana kadar yapılacak veritabanı işlemleri birikir.
                await _unitOfWork.SaveChangesAsync();

                //Başarılı bir şekilde silme işlemi yapıldıysa aşağıdaki mesajı döndürürüz.
                return new Result(ResultStatus.Success, $"{_department.Name} Başarıyla Silindi");
            }
            //Eğer departman null ise
            return new Result(ResultStatus.Error, "Seçili departman bulunamadı");
        }

        //Bu metot tüm departmanları getirecek. Bunu çoğunlukla listeleme işlemlerinde kullanırız.
        //ama bazen farklı işlemlerde de işimize yarayacak benzer metotlar yazabiliriz. 
        //Örneğin bir veritabanı tablosundan dropdownlist doldurmak için de kullanılabilir.
        public async Task<IDataResult<IList<Department>>> GetAll()
        {
            //Burada Tüm Departmanları alıyoruz. Alırken silinmiş olanları almıyoruz. Ayrıcı güncelleme tarihine göre sıralıyoruz.
            var departments = await _unitOfWork.Departments.GetAllAsync(
                predicate: d => !d.DeletedDate.HasValue,
                orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
            );

            //Eğer departman sayısı 0'dan büyükse
            if (departments.Count > -1)
            {
                //Departmanları döndürüyoruz.
                return new DataResult<IList<Department>>(ResultStatus.Success, departments);
            }
            //Eğer departman sayısı 0 ise hata döndürüyoruz.
            return new DataResult<IList<Department>>(ResultStatus.Error, "Hiç Departman bulunamadı", null);
        }

        //Bu metot departmanları sayfalı bir şekilde getirecek.
        public async Task<IDataResult<PageResponse<DepartmentListDto>>> GetToGrid(PageRequest request)
        {
            try
            {
                // İstekten gelen filtreyi kullanarak bir predicate oluşturuyoruz
                Expression<Func<Department, bool>> predicate = x => !x.DeletedDate.HasValue;
                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<Department>(request.Filter, deleted: false);

                // Sayfalama işlemi için GetEntitiesWithPaginationAsync metodunu kullanıyoruz
                var departments = await _unitOfWork.Departments.GetDtoWithPaginationAsync(
                    request.PageIndex, // İstekten gelen sayfa numarası
                    request.PageSize,  // İstekten gelen sayfa boyutu
                    predicate: predicate, // Silinmiş olmayan departmanları alırken, filtreyi de uyguluyoruz
                    orderBy: q => q.OrderByDescending(d => d.UpdatedDate) // Güncelleme tarihine göre azalan sırayla sıralıyoruz
                );

                foreach (var department in departments.Items)
                {
                    //department.CalismaTuru = (string)CalismaTuru
                }

                // Eğer departmanlar varsa, sayfalı yanıtı döndürüyoruz
                if (departments.Items.Any())
                {
                    return new DataResult<PageResponse<DepartmentListDto>>(ResultStatus.Success, departments);
                }

                // Eğer hiç departman yoksa hata mesajı döndürüyoruz
                return new DataResult<PageResponse<DepartmentListDto>>(ResultStatus.Error, "Hiç Departman bulunamadı", null);
            }
            catch (Exception ex)
            {
                // Hata durumunda, hata mesajı ile birlikte bir yanıt döndürüyoruz
                return new DataResult<PageResponse<DepartmentListDto>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }



        //Bu metot Id'si verilen departmanı getirecek.
        public async Task<IDataResult<Department>> GetById(int id)
        {
            //Id'si verilen departmanı getiriyoruz.
            var department = _unitOfWork.Departments.Get(id);

            //Eğer departman null değilse
            if (department != null)
            {
                //Departmanı döndürüyoruz.
                return new DataResult<Department>(ResultStatus.Success, department);//dto to entity yüzünden hata çıktı
            }
            //Eğer departman null ise hata döndürüyoruz.
            return new DataResult<Department>(ResultStatus.Error, "Departman bulunamadı", null);
        }


        private string? KodOlustur(string departmanAdi)
        {
            // İlk 5 karakteri al, eksikse 'X' ile tamamla
            string ilkBes = departmanAdi.ToUpper().PadRight(5, 'X').Substring(0, 5);

            // Rastgele 5 karakter oluştur
            string rastgeleKisim = RastgeleKodOlustur(5);

            // Birleştir
            string kod = $"{ilkBes}-{rastgeleKisim}";

            return kod;
        }

        public string RastgeleKodOlustur(int uzunluk)
        {
            const string karakterler = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < uzunluk; i++)
            {
                int index = random.Next(karakterler.Length);
                sb.Append(karakterler[index]);
            }

            return sb.ToString();
        }
    }
    }
