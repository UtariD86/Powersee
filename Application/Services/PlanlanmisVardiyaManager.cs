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

    public class PlanlanmisVardiyaManager : IPlanlanmisVardiyaService
    {

        private readonly IUnitOfWork _unitOfWork;


        private readonly FilterHelper _filterHelper;


        public PlanlanmisVardiyaManager(IUnitOfWork unitOfWork, FilterHelper filterHelper)
        {

            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;


        }


        public async Task<IDataResult<PlanlanmisVardiya>> Edit(PlanlanmisVardiya planlanmisvardiya)
        {

            try
            {

                if (planlanmisvardiya != null && planlanmisvardiya.Id != 0)
                {

                    planlanmisvardiya.UpdatedDate = DateTime.UtcNow;

                    await _unitOfWork.PlanlanmisVardiyalar.UpdateAsync(planlanmisvardiya);


                    await _unitOfWork.SaveChangesAsync();


                    return new DataResult<PlanlanmisVardiya>(
                        data: planlanmisvardiya,
                        resultStatus: ResultStatus.Success,
                        message: $"{planlanmisvardiya.baslangicZamani} Başlangıç {planlanmisvardiya.bitisZamani} Bitiş Tarihli Planlanmış Vardiya Güncellendi"
                        );
                }
                else
                {

                    planlanmisvardiya.UpdatedDate = DateTime.UtcNow;
                    planlanmisvardiya.CreatedDate = DateTime.UtcNow;


                    await _unitOfWork.PlanlanmisVardiyalar.AddAsync(planlanmisvardiya);


                    await _unitOfWork.SaveChangesAsync();

                    return new DataResult<PlanlanmisVardiya>(ResultStatus.Success, planlanmisvardiya.baslangicZamani + "Başlangıç Tarihli Planlanmış Vardiya Başarıyla Eklendi", planlanmisvardiya);
                }
            }
            catch (Exception ex)
            {

                return new DataResult<PlanlanmisVardiya>(
                   resultStatus: ResultStatus.Error,
                   message: $"Malesef bir sorun oluştu...",
                   exception: ex,
                   data: null
                   );
            }
        }


        public async Task<IResult> Delete(int Id)
        {

            var _planlanmisvardiya = _unitOfWork.PlanlanmisVardiyalar.Get(Id);

            if (_planlanmisvardiya != null)
            {

                _planlanmisvardiya.DeletedDate = DateTime.UtcNow;


                _planlanmisvardiya.UpdatedDate = DateTime.UtcNow;


                await _unitOfWork.PlanlanmisVardiyalar.UpdateAsync(_planlanmisvardiya);


                await _unitOfWork.SaveChangesAsync();


                return new Result(ResultStatus.Success, $"{_planlanmisvardiya.baslangicZamani} Başlangıç Tarihli Planlanmış Vardiya Başarıyla Silindi");
            }

            return new Result(ResultStatus.Error, "Seçili şube bulunamadı");
        }


        public async Task<IDataResult<IList<PlanlanmisVardiya>>> GetAll()
        {

            var planlanmisvardiyalar = await _unitOfWork.PlanlanmisVardiyalar.GetAllAsync(
                predicate: d => !d.DeletedDate.HasValue,
                orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
            );


            if (planlanmisvardiyalar.Count > -1)
            {

                return new DataResult<IList<PlanlanmisVardiya>>(ResultStatus.Success, planlanmisvardiyalar);
            }

            return new DataResult<IList<PlanlanmisVardiya>>(ResultStatus.Error, "Hiç Şube bulunamadı", null);
        }


        public async Task<IDataResult<PageResponse<PlanlanmisVardiya>>> GetToGrid(PageRequest request)
        {
            try
            {

                Expression<Func<PlanlanmisVardiya, bool>> predicate = x => !x.DeletedDate.HasValue;
                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<PlanlanmisVardiya>(request.Filter, deleted: false);


                var planlanmisvardiyalar = await _unitOfWork.PlanlanmisVardiyalar.GetEntitiesWithPaginationAsync(
                    request.PageIndex,
                    request.PageSize,
                    predicate: predicate,
                    orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
                );


                if (planlanmisvardiyalar.Items.Any())
                {
                    return new DataResult<PageResponse<PlanlanmisVardiya>>(ResultStatus.Success, planlanmisvardiyalar);
                }


                return new DataResult<PageResponse<PlanlanmisVardiya>>(ResultStatus.Error, "Hiç Şube bulunamadı", null);
            }
            catch (Exception ex)
            {

                return new DataResult<PageResponse<PlanlanmisVardiya>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }




        public async Task<IDataResult<PlanlanmisVardiya>> GetById(int id)
        {

            var planlanmisvardiya = _unitOfWork.PlanlanmisVardiyalar.Get(id);


            if (planlanmisvardiya != null)
            {

                return new DataResult<PlanlanmisVardiya>(ResultStatus.Success, planlanmisvardiya);
            }

            return new DataResult<PlanlanmisVardiya>(ResultStatus.Error, "Şube bulunamadı", null);
        }



    }
}
