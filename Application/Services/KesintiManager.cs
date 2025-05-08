


// BU SAYFA TAMAMEN YAPAY ZEKA TARAFINDAN YAZILDI. AA



using Application.Helpers.Concrete.Filtering; // FilterHelper için
using Application.Services.Abstract; // IKesintiService için
using Core.Dtos.Abstract; // IDataResult, IResult için
using Core.Dtos.Concrete; // PageResponse, PageRequest, DataResult, Result için
using Core.Enums; // ResultStatus için
using Domain.Dtos;

// KesintiListDto'nun tanımlanacağı varsayılan namespace (Adım 19'da oluşturulacak)
// using Domain.Dtos; // Veya DTO'ların bulunduğu namespace
using Domain.Entities; // Kesinti entity'si için
using Persistence.Abstract; // IUnitOfWork için
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

// Namespace'i diğer Manager'larla aynı tuttuk
namespace Application.Services
{
    public class KesintiManager : IKesintiService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FilterHelper _filterHelper;

        public KesintiManager(IUnitOfWork unitOfWork, FilterHelper filterHelper)
        {
            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;
        }

        // Ekleme/Güncelleme Metodu
        public async Task<IDataResult<Kesinti>> Edit(Kesinti kesinti)
        {
            try
            {
                // --- Foreign Key Doğrulaması ---
                // PersonelId'nin geçerli olup olmadığını kontrol et (Asenkron kontrol)
                var personelExists = await _unitOfWork.Personels.AnyAsync(p => p.Id == kesinti.PersonelId && !p.DeletedDate.HasValue);
                if (!personelExists)
                {
                    return new DataResult<Kesinti>(
                              resultStatus: ResultStatus.Error,
                              message: "Seçilen Personel Geçersiz!",
                              data: null);
                }

                // PlanlanmisVardiyaSnapshotId kontrolü (Yorumlu)
                // var snapshotExists = await _unitOfWork.PlanlananVardiyaSnapshots.AnyAsync(s => s.Id == kesinti.PlanlanmisVardiyaSnapshotId);
                // if (!snapshotExists)
                // {
                //     return new DataResult<Kesinti>(ResultStatus.Error, $"ID: {kesinti.PlanlanmisVardiyaSnapshotId} olan geçerli bir vardiya snapshot bulunamadı.", null);
                // }
                // --- Doğrulama Sonu ---


                if (kesinti != null && kesinti.Id != 0) // Güncelleme
                {
                    kesinti.UpdatedDate = DateTime.UtcNow;

                    await _unitOfWork.Kesintiler.UpdateAsync(kesinti);
                    await _unitOfWork.SaveChangesAsync();

                    return new DataResult<Kesinti>(
                        data: kesinti,
                        resultStatus: ResultStatus.Success,
                        message: $"ID: {kesinti.Id} olan kesinti başarıyla güncellendi."
                        );
                }
                else // Ekleme
                {
                    if (kesinti == null)
                        return new DataResult<Kesinti>(ResultStatus.Error, "Eklenecek kesinti bilgisi boş olamaz.", null);

                    kesinti.UpdatedDate = DateTime.UtcNow;
                    kesinti.CreatedDate = DateTime.UtcNow;
                    kesinti.DeletedDate = null;

                    await _unitOfWork.Kesintiler.AddAsync(kesinti);
                    await _unitOfWork.SaveChangesAsync();

                    return new DataResult<Kesinti>(
                        resultStatus: ResultStatus.Success,
                        message: "Kesinti başarıyla eklendi.",
                        data: kesinti);
                }
            }
            catch (Exception ex)
            {
                return new DataResult<Kesinti>(
                    resultStatus: ResultStatus.Error,
                    message: $"Kesinti kaydedilirken bir sorun oluştu: {ex.Message}",
                    exception: ex,
                    data: null
                    );
            }
        }

        // Silme Metodu
        public async Task<IResult> Delete(int Id)
        {
            // DÜZELTME: Get metoduna lambda yerine sadece Id gönderildi
            var _kesinti = _unitOfWork.Kesintiler.Get(Id); // Senkron Get(Id)

            if (_kesinti != null)
            {
                if (_kesinti.DeletedDate.HasValue)
                    return new Result(ResultStatus.Error, $"ID: {Id} olan kesinti zaten silinmiş.");

                _kesinti.DeletedDate = DateTime.UtcNow;
                _kesinti.UpdatedDate = DateTime.UtcNow;

                await _unitOfWork.Kesintiler.UpdateAsync(_kesinti);
                await _unitOfWork.SaveChangesAsync();

                return new Result(ResultStatus.Success, $"ID: {_kesinti.Id} olan kesinti başarıyla silindi.");
            }
            // DÜZELTME: NotFound yerine Error kullanıldı
            return new Result(ResultStatus.Error, "Silinecek kesinti bulunamadı.");
        }

        // Tümünü Getirme Metodu
        public async Task<IDataResult<IList<Kesinti>>> GetAll()
        {
            var kesintiler = await _unitOfWork.Kesintiler.GetAllAsync(
                predicate: k => !k.DeletedDate.HasValue,
                orderBy: q => q.OrderByDescending(k => k.UpdatedDate)
            );

            if (kesintiler != null && kesintiler.Any()) // Any() kontrolü eklendi
            {
                return new DataResult<IList<Kesinti>>(ResultStatus.Success, kesintiler);
            }
            // DÜZELTME: NotFound yerine Error kullanıldı, mesaj güncellendi
            return new DataResult<IList<Kesinti>>(ResultStatus.Error, "Gösterilecek kesinti bulunamadı.", new List<Kesinti>());
        }

        // Grid İçin Sayfalı Getirme Metodu (Şimdilik Entity döndürüyor)
        public async Task<IDataResult<PageResponse<KesintiListDto>>> GetToGrid(PageRequest request)
        {
            try
            {
                Expression<Func<Kesinti, bool>> predicate = x => !x.DeletedDate.HasValue;

                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<Kesinti>(request.Filter, deleted: false);

                var kesintis = await _unitOfWork.Kesintiler.GetAllKesintisAsync(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    predicate: predicate,
                    orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
                );

                if (kesintis.Items.Any())
                {
                    return new DataResult<PageResponse<KesintiListDto>>(ResultStatus.Success, kesintis);
                }

                return new DataResult<PageResponse<KesintiListDto>>(ResultStatus.Error, "Hiç talep bulunamadı", null);
            }
            catch (Exception ex)
            {
                return new DataResult<PageResponse<KesintiListDto>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }


        // ID ile Getirme Metodu
        public async Task<IDataResult<Kesinti>> GetById(int id)
        {
            // DÜZELTME: Get metoduna lambda yerine sadece Id gönderildi
            var kesinti = _unitOfWork.Kesintiler.Get(id); // Senkron Get(id)

            if (kesinti != null)
            {
                if (!kesinti.DeletedDate.HasValue)
                    return new DataResult<Kesinti>(ResultStatus.Success, kesinti);
                else
                    // DÜZELTME: NotFound yerine Error kullanıldı
                    return new DataResult<Kesinti>(ResultStatus.Error, "Kesinti bulunamadı (silinmiş).", null);
            }
            // DÜZELTME: NotFound yerine Error kullanıldı
            return new DataResult<Kesinti>(ResultStatus.Error, "Kesinti bulunamadı", null);
        }
    }
}
