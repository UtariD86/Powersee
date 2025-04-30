using Application.Helpers.Concrete.Filtering;
using Application.Services.Abstract;
using Azure.Core;
using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using DocumentFormat.OpenXml.Bibliography;
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

    public class VardiyaManager : IVardiyaService
    {

        private readonly IUnitOfWork _unitOfWork;


        private readonly FilterHelper _filterHelper;


        public VardiyaManager(IUnitOfWork unitOfWork, FilterHelper filterHelper)
        {

            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;


        }


        public async Task<IDataResult<Vardiya>> Edit(Vardiya vardiya)
        {

            try
            {

                if (vardiya != null && vardiya.Id != 0)
                {

                    vardiya.UpdatedDate = DateTime.UtcNow;

                    await _unitOfWork.Vardiyalar.UpdateAsync(vardiya);


                    await _unitOfWork.SaveChangesAsync();


                    return new DataResult<Vardiya>(
                        data: vardiya,
                        resultStatus: ResultStatus.Success,
                        message: $"{vardiya.vardiyaIsmi} isimli vardiya başarıyla güncellendi."
                        );
                }
                else
                {

                    vardiya.UpdatedDate = DateTime.UtcNow;
                    vardiya.CreatedDate = DateTime.UtcNow;


                    await _unitOfWork.Vardiyalar.AddAsync(vardiya);


                    await _unitOfWork.SaveChangesAsync();

                    return new DataResult<Vardiya>(ResultStatus.Success, vardiya.vardiyaIsmi + "Başarıyla Eklendi", vardiya);
                }
            }
            catch (Exception ex)
            {

                return new DataResult<Vardiya>(
                   resultStatus: ResultStatus.Error,
                   message: $"Malesef bir sorun oluştu...",
                   exception: ex,
                   data: null
                   );
            }
        }


        public async Task<IResult> Delete(int Id)
        {

            var _vardiya = _unitOfWork.Vardiyalar.Get(Id);

            if (_vardiya != null)
            {

                _vardiya.DeletedDate = DateTime.UtcNow;


                _vardiya.UpdatedDate = DateTime.UtcNow;


                await _unitOfWork.Vardiyalar.UpdateAsync(_vardiya);


                await _unitOfWork.SaveChangesAsync();


                return new Result(ResultStatus.Success, $"{_vardiya.vardiyaIsmi} Başarıyla Silindi");
            }

            return new Result(ResultStatus.Error, "Seçili vardiya bulunamadı");
        }


        public async Task<IDataResult<IList<Vardiya>>> GetAll()
        {

            var vardiyalar = await _unitOfWork.Vardiyalar.GetAllAsync(
                predicate: d => !d.DeletedDate.HasValue,
                orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
            );


            if (vardiyalar.Count > -1)
            {

                return new DataResult<IList<Vardiya>>(ResultStatus.Success, vardiyalar);
            }

            return new DataResult<IList<Vardiya>>(ResultStatus.Error, "Hiç vardiya bulunamadı", null);
        }


        public async Task<IDataResult<PageResponse<Vardiya>>> GetToGrid(PageRequest request)
        {
            try
            {

                Expression<Func<Vardiya, bool>> predicate = x => !x.DeletedDate.HasValue;
                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<Vardiya>(request.Filter, deleted: false);


                var vardiyalar = await _unitOfWork.Vardiyalar.GetEntitiesWithPaginationAsync(
                    request.PageIndex,
                    request.PageSize,
                    predicate: predicate,
                    orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
                );


                if (vardiyalar.Items.Any())
                {
                    return new DataResult<PageResponse<Vardiya>>(ResultStatus.Success, vardiyalar);
                }


                return new DataResult<PageResponse<Vardiya>>(ResultStatus.Error, "Hiç vardiya bulunamadı", null);
            }
            catch (Exception ex)
            {

                return new DataResult<PageResponse<Vardiya>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }




        public async Task<IDataResult<Vardiya>> GetById(int id)
        {

            var vardiya = _unitOfWork.Vardiyalar.Get(id);


            if (vardiya != null)
            {

                return new DataResult<Vardiya>(ResultStatus.Success, vardiya);
            }

            return new DataResult<Vardiya>(ResultStatus.Error, "Vardiya bulunamadı", null);
        }

    }
}
