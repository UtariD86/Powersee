using Application.Helpers.Concrete.Filtering;
using Application.Services.Abstract;
using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using Domain.Dtos;
using Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;


namespace Application.Services
{
    public class TalepManager : ITalepService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FilterHelper _filterHelper;

        public TalepManager(IUnitOfWork unitOfWork, FilterHelper filterHelper)
        {
            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;
        }

        public async Task<IDataResult<Talep>> Edit(Talep talep)
        {
            try
            {
                if (talep != null && talep.Id != 0)
                {

                    talep.UpdatedDate = DateTime.UtcNow;

                    await _unitOfWork.Taleps.UpdateAsync(talep);
                    await _unitOfWork.SaveChangesAsync();
                    return new DataResult<Talep>(ResultStatus.Success, $"Talep başarıyla güncellendi.", talep);
                }
                else
                {
                    talep.UpdatedDate = DateTime.UtcNow;
                    talep.CreatedDate = DateTime.UtcNow;

                   
                    talep.CreatedDate = DateTime.UtcNow;
                    await _unitOfWork.Taleps.AddAsync(talep);
                    await _unitOfWork.SaveChangesAsync();
                    return new DataResult<Talep>(ResultStatus.Success, "Talep başarıyla eklendi.", talep);
                }
            }
            catch (Exception ex)
            {
                return new DataResult<Talep>(
                  resultStatus: ResultStatus.Error,
                  message: $"Malesef bir sorun oluştu...",
                  exception: ex,
                  data: null
                  );
            }
        }

        public async Task<IResult> Delete(int id)
        {
            var talep = await _unitOfWork.Taleps.GetAsync(p => p.Id == id);
            if (talep != null)
            {
                talep.DeletedDate = DateTime.UtcNow;
                talep.UpdatedDate = DateTime.UtcNow;
                await _unitOfWork.Taleps.UpdateAsync(talep);
                await _unitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, "Talep başarıyla silindi.");
            }
            return new Result(ResultStatus.Error, "Talep bulunamadı.");
        }

        public async Task<IDataResult<IList<Talep>>> GetAll()
        {
            var taleps = await _unitOfWork.Taleps.GetAllAsync(p => !p.DeletedDate.HasValue, q => q.OrderByDescending(p => p.UpdatedDate));
            if (taleps.Count > -1)
            {
                return new DataResult<IList<Talep>>(ResultStatus.Success, taleps);
            }
            return new DataResult<IList<Talep>>(ResultStatus.Error, "Hiç talep bulunamadı.", null);
        }

     
        public async Task<IDataResult<Talep>> GetById(int id)
        {
            var talep = await _unitOfWork.Taleps.GetAsync(p => p.Id == id);
            if (talep != null)
            {
                return new DataResult<Talep>(ResultStatus.Success, talep);
            }
            return new DataResult<Talep>(ResultStatus.Error, "Talep bulunamadı.", null);
        }

        private string GenerateUniqueTalepCode(string name)
        {
            var shortName = name.Length >= 5 ? name.Substring(0, 5).ToUpper() : name.ToUpper();
            var randomString = Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper();
            return $"{shortName}-{randomString}";
        }

        public async Task<IDataResult<PageResponse<TalepDetailsDto>>> GetToGrid(PageRequest request)
        {
            try
            {
                Expression<Func<Talep, bool>> predicate = x => !x.DeletedDate.HasValue;

                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<Talep>(request.Filter, deleted: false);

                var taleps = await _unitOfWork.Taleps.GetAllTalepsAsync(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    predicate: predicate,
                    orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
                );

                if (taleps.Items.Any())
                {
                    return new DataResult<PageResponse<TalepDetailsDto>>(ResultStatus.Success, taleps);
                }

                return new DataResult<PageResponse<TalepDetailsDto>>(ResultStatus.Error, "Hiç talep bulunamadı", null);
            }
            catch (Exception ex)
            {
                return new DataResult<PageResponse<TalepDetailsDto>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }

     
    }
}

