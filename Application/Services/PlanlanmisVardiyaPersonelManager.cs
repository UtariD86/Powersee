using Application.Helpers.Concrete.Filtering;
using Application.Services.Abstract;
using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using Domain.Entities;
using Persistence.Abstract;
using Persistence.Concrete.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PlanlanmisVardiyaPersonelManager : IPlanlanmisVardiyaPersonelService
    {

        private readonly IUnitOfWork _unitOfWork;


        private readonly FilterHelper _filterHelper;


        public PlanlanmisVardiyaPersonelManager(IUnitOfWork unitOfWork, FilterHelper filterHelper)
        {

            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;


        }


        public async Task<IDataResult<PlanlanmisVardiyaPersonel>> Edit(PlanlanmisVardiyaPersonel PlanlanmisVardiyaPersonel)
        {

            try
            {

                if (PlanlanmisVardiyaPersonel != null && PlanlanmisVardiyaPersonel.Id != 0)
                {

                    PlanlanmisVardiyaPersonel.UpdatedDate = DateTime.UtcNow;

                    await _unitOfWork.PlanlanmisVardiyaPersoneller.UpdateAsync(PlanlanmisVardiyaPersonel);


                    await _unitOfWork.SaveChangesAsync();


                    return new DataResult<PlanlanmisVardiyaPersonel>(
                        data: PlanlanmisVardiyaPersonel,
                        resultStatus: ResultStatus.Success,
                        message: $"Başarıyla güncellendi."
                        );
                }
                else
                {

                    PlanlanmisVardiyaPersonel.UpdatedDate = DateTime.UtcNow;
                    PlanlanmisVardiyaPersonel.CreatedDate = DateTime.UtcNow;


                    await _unitOfWork.PlanlanmisVardiyaPersoneller.AddAsync(PlanlanmisVardiyaPersonel);


                    await _unitOfWork.SaveChangesAsync();

                    return new DataResult<PlanlanmisVardiyaPersonel>(ResultStatus.Success, "Başarıyla Eklendi", PlanlanmisVardiyaPersonel);
                }
            }
            catch (Exception ex)
            {

                return new DataResult<PlanlanmisVardiyaPersonel>(
                   resultStatus: ResultStatus.Error,
                   message: $"Malesef bir sorun oluştu...",
                   exception: ex,
                   data: null
                   );
            }
        }


        public async Task<IResult> Delete(int Id)
        {

            var _PlanlanmisVardiyaPersonel = _unitOfWork.PlanlanmisVardiyaPersoneller.Get(Id);

            if (_PlanlanmisVardiyaPersonel != null)
            {

                _PlanlanmisVardiyaPersonel.DeletedDate = DateTime.UtcNow;


                _PlanlanmisVardiyaPersonel.UpdatedDate = DateTime.UtcNow;


                await _unitOfWork.PlanlanmisVardiyaPersoneller.UpdateAsync(_PlanlanmisVardiyaPersonel);


                await _unitOfWork.SaveChangesAsync();


                return new Result(ResultStatus.Success, $"Başarıyla Silindi");
            }

            return new Result(ResultStatus.Error, "Seçili şube bulunamadı");
        }


        public async Task<IDataResult<IList<PlanlanmisVardiyaPersonel>>> GetAll()
        {

            var PlanlanmisVardiyaPersoneller = await _unitOfWork.PlanlanmisVardiyaPersoneller.GetAllAsync(
                predicate: d => !d.DeletedDate.HasValue,
                orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
            );


            if (PlanlanmisVardiyaPersoneller.Count > -1)
            {

                return new DataResult<IList<PlanlanmisVardiyaPersonel>>(ResultStatus.Success, PlanlanmisVardiyaPersoneller);
            }

            return new DataResult<IList<PlanlanmisVardiyaPersonel>>(ResultStatus.Error, "Hiç Şube bulunamadı", null);
        }



        public async Task<IDataResult<IList<PlanlanmisVardiyaPersonel>>> TryOperation(bool isGiris, int vardiyaId, int PersonelId)
        {
            var PlanlanmisVardiyaPersoneller = await _unitOfWork.PlanlanmisVardiyaPersoneller.GetAllAsync(
                predicate: d => !d.DeletedDate.HasValue && d.PlanlanmisVardiyaId == vardiyaId && d.PersonelId == PersonelId,
                orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
            );

            // Eğer hiç kayıt bulunmazsa, hata mesajı döndür
            if (PlanlanmisVardiyaPersoneller.Count == 0)
            {
                return new DataResult<IList<PlanlanmisVardiyaPersonel>>(ResultStatus.Error, "Hiç Şube bulunamadı", null);
            }

            var personel = PlanlanmisVardiyaPersoneller.FirstOrDefault();

            // Giris ve cikis zamanları mevcutsa kontrol et
            if (isGiris)
            {
                if (personel.girisZamani == null)
                {
                    personel.girisZamani = DateTime.Now; // Eğer giriş zamanı yoksa, DateTime.Now ata
                                                         // Burada veritabanına güncelleme yapmanız gerekebilir.
                }
            }
            else
            {
                if (personel.cikisZamani == null)
                {
                    personel.cikisZamani = DateTime.Now; // Eğer çıkış zamanı yoksa, DateTime.Now ata
                                                         // Burada veritabanına güncelleme yapmanız gerekebilir.
                }
            }

            // Güncellenen PlanlanmisVardiyaPersonel'i veritabanında kaydet
            await _unitOfWork.PlanlanmisVardiyaPersoneller.UpdateAsync(personel);

            return new DataResult<IList<PlanlanmisVardiyaPersonel>>(ResultStatus.Success, PlanlanmisVardiyaPersoneller);
        }



        public async Task<List<int>> GetAllPersonelIds(int vardiyaId)
        {

            var PlanlanmisVardiyaPersoneller = await _unitOfWork.PlanlanmisVardiyaPersoneller.GetAllAsync(
                predicate: d => !d.DeletedDate.HasValue && d.PlanlanmisVardiyaId == vardiyaId,
                orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
            );


            if (PlanlanmisVardiyaPersoneller.Count > 0)
            {
                var IdList = PlanlanmisVardiyaPersoneller.Select(x => x.PersonelId).ToList();
                return IdList;
            }

            return new();
        }


        public async Task<IDataResult<PageResponse<PlanlanmisVardiyaPersonel>>> GetToGrid(PageRequest request)
        {
            try
            {

                Expression<Func<PlanlanmisVardiyaPersonel, bool>> predicate = x => !x.DeletedDate.HasValue;
                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<PlanlanmisVardiyaPersonel>(request.Filter, deleted: false);


                var PlanlanmisVardiyaPersoneller = await _unitOfWork.PlanlanmisVardiyaPersoneller.GetEntitiesWithPaginationAsync(
                    request.PageIndex,
                    request.PageSize,
                    predicate: predicate,
                    orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
                );


                if (PlanlanmisVardiyaPersoneller.Items.Any())
                {
                    return new DataResult<PageResponse<PlanlanmisVardiyaPersonel>>(ResultStatus.Success, PlanlanmisVardiyaPersoneller);
                }


                return new DataResult<PageResponse<PlanlanmisVardiyaPersonel>>(ResultStatus.Error, "Hiç Şube bulunamadı", null);
            }
            catch (Exception ex)
            {

                return new DataResult<PageResponse<PlanlanmisVardiyaPersonel>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }




        public async Task<IDataResult<PlanlanmisVardiyaPersonel>> GetById(int id)
        {

            var PlanlanmisVardiyaPersonel = _unitOfWork.PlanlanmisVardiyaPersoneller.Get(id);


            if (PlanlanmisVardiyaPersonel != null)
            {

                return new DataResult<PlanlanmisVardiyaPersonel>(ResultStatus.Success, PlanlanmisVardiyaPersonel);
            }

            return new DataResult<PlanlanmisVardiyaPersonel>(ResultStatus.Error, "Şube bulunamadı", null);
        }


        public async Task<IResult> UpdateVardiyaPersonel(int vardiyaId, List<int> personelIds)
        {
            try
            {
                // VardiyaId'ye göre mevcut planlanmış personelleri al
                var existingVardiyaPersoneller = await _unitOfWork.PlanlanmisVardiyaPersoneller.GetAllAsync(
                    predicate: d => d.PlanlanmisVardiyaId == vardiyaId && !d.DeletedDate.HasValue
                );

                // Mevcut personelId'leri veritabanından çıkar
                var existingPersonelIds = existingVardiyaPersoneller.Select(x => x.PersonelId).ToList();

                // Eklenmesi gereken personelId'leri bulun
                var personelToAdd = personelIds.Except(existingPersonelIds).ToList();

                // Silinmesi gereken personelId'leri bulun
                var personelToDelete = existingPersonelIds.Except(personelIds).ToList();

                // Eksik olan personelleri ekle
                foreach (var personelId in personelToAdd)
                {
                    var newVardiyaPersonel = new PlanlanmisVardiyaPersonel
                    {
                        PersonelId = personelId,
                        PlanlanmisVardiyaId = vardiyaId,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    };
                    await _unitOfWork.PlanlanmisVardiyaPersoneller.AddAsync(newVardiyaPersonel);
                }

                // Fazla olan personelleri sil
                foreach (var personelId in personelToDelete)
                {
                    var personelToRemove = existingVardiyaPersoneller.FirstOrDefault(x => x.PersonelId == personelId);
                    if (personelToRemove != null)
                    {
                        personelToRemove.DeletedDate = DateTime.UtcNow;
                        personelToRemove.UpdatedDate = DateTime.UtcNow;
                        await _unitOfWork.PlanlanmisVardiyaPersoneller.UpdateAsync(personelToRemove);
                    }
                }

                // Değişiklikleri kaydet
                await _unitOfWork.SaveChangesAsync();

                return new Result(ResultStatus.Success, "Vardiya personel ilişkileri başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                return new Result(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}");
            }
        }

    }
}
