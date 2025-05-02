using Core.Helpers.Abstract;
using Application.Helpers.Concrete;
using Application.Helpers.Concrete.Filtering;
using Application.Services.Abstract;
using Azure.Core;
using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Bibliography;
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

    public class IzinManager : IIzinService
    {

        private readonly IUnitOfWork _unitOfWork;


        private readonly FilterHelper _filterHelper;


        public IzinManager(IUnitOfWork unitOfWork, FilterHelper filterHelper)
        {

            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;


        }


        public async Task<IDataResult<Izin>> Edit(Izin izin)
        {

            try
            {

                if (izin != null && izin.Id != 0)
                {

                    izin.UpdatedDate = DateTime.UtcNow;

                    await _unitOfWork.Izinler.UpdateAsync(izin);


                    await _unitOfWork.SaveChangesAsync();


                    return new DataResult<Izin>(
                        data: izin,
                        resultStatus: ResultStatus.Success,
                        message: $"{izin.IzinTuruEnum} isimli izin başarıyla güncellendi."
                        );
                }
                else
                {

                    izin.UpdatedDate = DateTime.UtcNow;
                    izin.CreatedDate = DateTime.UtcNow;


                    await _unitOfWork.Izinler.AddAsync(izin);


                    await _unitOfWork.SaveChangesAsync();

                    return new DataResult<Izin>(ResultStatus.Success, izin.IzinTuruEnum + "Başarıyla Eklendi", izin);
                }
            }
            catch (Exception ex)
            {

                return new DataResult<Izin>(
                   resultStatus: ResultStatus.Error,
                   message: $"Malesef bir sorun oluştu...",
                   exception: ex,
                   data: null
                   );
            }
        }


        public async Task<IResult> Delete(int Id)
        {

            var _izin = _unitOfWork.Izinler.Get(Id);

            if (_izin != null)
            {

                _izin.DeletedDate = DateTime.UtcNow;


                _izin.UpdatedDate = DateTime.UtcNow;


                await _unitOfWork.Izinler.UpdateAsync(_izin);


                await _unitOfWork.SaveChangesAsync();


                return new Result(ResultStatus.Success, $"{_izin.IzinTuruEnum} Başarıyla Silindi");
            }

            return new Result(ResultStatus.Error, "Seçili izin bulunamadı");
        }


        public async Task<IDataResult<IList<Izin>>> GetAll()
        {

            var personels = await _unitOfWork.Izinler.GetAllAsync(
                predicate: d => !d.DeletedDate.HasValue,
                orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
            );


            if (personels.Count > -1)
            {

                return new DataResult<IList<Izin>>(ResultStatus.Success, personels);
            }

            return new DataResult<IList<Izin>>(ResultStatus.Error, "Hiç İzin bulunamadı", null);
        }


        public async Task<IDataResult<PageResponse<IzinListDto>>> GetToGrid(PageRequest request)
        {
            try
            {

                Expression<Func<Izin, bool>> predicate = x => !x.DeletedDate.HasValue;
                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<Izin>(request.Filter, deleted: false);


                var izinler = await _unitOfWork.Izinler.GetAllIzinsAsync(
                    predicate: predicate,
                    orderBy: q => q.OrderByDescending(d => d.UpdatedDate),
                    request.PageIndex,
                    request.PageSize
                );


                if (izinler.Items.Any())
                {
                    return new DataResult<PageResponse<IzinListDto>>(ResultStatus.Success, izinler);
                }


                return new DataResult<PageResponse<IzinListDto>>(ResultStatus.Error, "Hiç Izin bulunamadı", null);
            }
            catch (Exception ex)
            {

                return new DataResult<PageResponse<IzinListDto>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }




        public async Task<IDataResult<Izin>> GetById(int id)
        {

            var izin = _unitOfWork.Izinler.Get(id);


            if (izin != null)
            {

                return new DataResult<Izin>(ResultStatus.Success, izin);
            }

            return new DataResult<Izin>(ResultStatus.Error, "Personel bulunamadı", null);
        }

    }
}
