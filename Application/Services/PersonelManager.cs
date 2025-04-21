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

    public class PersonelManager : IPersonelService
    {

        private readonly IUnitOfWork _unitOfWork;


        private readonly FilterHelper _filterHelper;


        public PersonelManager(IUnitOfWork unitOfWork, FilterHelper filterHelper)
        {

            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;


        }


        public async Task<IDataResult<Personel>> Edit(Personel personel)
        {

            try
            {

                if (personel != null && personel.Id != 0)
                {

                    personel.UpdatedDate = DateTime.UtcNow;

                    await _unitOfWork.Personels.UpdateAsync(personel);


                    await _unitOfWork.SaveChangesAsync();


                    return new DataResult<Personel>(
                        data: personel,
                        resultStatus: ResultStatus.Success,
                        message: $"{personel.isim} isimli şube başarıyla güncellendi."
                        );
                }
                else
                {

                    personel.UpdatedDate = DateTime.UtcNow;
                    personel.CreatedDate = DateTime.UtcNow;


                    await _unitOfWork.Personels.AddAsync(personel);


                    await _unitOfWork.SaveChangesAsync();

                    return new DataResult<Personel>(ResultStatus.Success, personel.isim + "Başarıyla Eklendi", personel);
                }
            }
            catch (Exception ex)
            {

                return new DataResult<Personel>(
                   resultStatus: ResultStatus.Error,
                   message: $"Malesef bir sorun oluştu...",
                   exception: ex,
                   data: null
                   );
            }
        }


        public async Task<IResult> Delete(int Id)
        {

            var _personel = _unitOfWork.Personels.Get(Id);

            if (_personel != null)
            {

                _personel.DeletedDate = DateTime.UtcNow;


                _personel.UpdatedDate = DateTime.UtcNow;


                await _unitOfWork.Personels.UpdateAsync(_personel);


                await _unitOfWork.SaveChangesAsync();


                return new Result(ResultStatus.Success, $"{_personel.isim} Başarıyla Silindi");
            }

            return new Result(ResultStatus.Error, "Seçili personel bulunamadı");
        }


        public async Task<IDataResult<IList<Personel>>> GetAll()
        {

            var personels = await _unitOfWork.Personels.GetAllAsync(
                predicate: d => !d.DeletedDate.HasValue,
                orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
            );


            if (personels.Count > -1)
            {

                return new DataResult<IList<Personel>>(ResultStatus.Success, personels);
            }

            return new DataResult<IList<Personel>>(ResultStatus.Error, "Hiç Şube bulunamadı", null);
        }


        public async Task<IDataResult<PageResponse<PersonelListDto>>> GetToGrid(PageRequest request)
        {
            try
            {

                Expression<Func<Personel, bool>> predicate = x => !x.DeletedDate.HasValue;
                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<Personel>(request.Filter, deleted: false);


                var personels = await _unitOfWork.Personels.GetAllPersonelsAsync(
                    predicate: predicate,
                    orderBy: q => q.OrderByDescending(d => d.UpdatedDate),
                    request.PageIndex,
                    request.PageSize
                );


                if (personels.Items.Any())
                {
                    return new DataResult<PageResponse<PersonelListDto>>(ResultStatus.Success, personels);
                }


                return new DataResult<PageResponse<PersonelListDto>>(ResultStatus.Error, "Hiç Şube bulunamadı", null);
            }
            catch (Exception ex)
            {

                return new DataResult<PageResponse<PersonelListDto>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }




        public async Task<IDataResult<Personel>> GetById(int id)
        {

            var personel = _unitOfWork.Personels.Get(id);


            if (personel != null)
            {

                return new DataResult<Personel>(ResultStatus.Success, personel);
            }

            return new DataResult<Personel>(ResultStatus.Error, "Şube bulunamadı", null);
        }

    }
}
