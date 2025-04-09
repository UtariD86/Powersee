using Application.Helpers.Concrete.Filtering;
using Application.Services.Abstract;
using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Core.Enums;
using Domain.Entities;
using Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PositionManager : IPositionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FilterHelper _filterHelper;

        public PositionManager(IUnitOfWork unitOfWork, FilterHelper filterHelper)
        {
            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;
        }

        public async Task<IDataResult<Position>> Edit(Position position)
        {
            try
            {
                if (position != null && position.Id != 0)
                {
                    
                    await _unitOfWork.Positions.UpdateAsync(position);
                    await _unitOfWork.SaveChangesAsync();
                    return new DataResult<Position>(ResultStatus.Success, $"{position.Name} başarıyla güncellendi.", position);
                }
                else
                {
                    var checkPosition = await _unitOfWork.Positions.GetAsync(p => p.Name == position.Name);
                    if (checkPosition != null)
                    {
                        return new DataResult<Position>(ResultStatus.Error, "Bu pozisyon zaten mevcut.", null);
                    }

                   position.Code=GenerateUniquePositionCode(position.Name);
                    position.CreatedDate = DateTime.UtcNow;
                    await _unitOfWork.Positions.AddAsync(position);
                    await _unitOfWork.SaveChangesAsync();
                    return new DataResult<Position>(ResultStatus.Success, "Pozisyon başarıyla eklendi.", position);
                }
            }
            catch (Exception ex)
            {
                return new DataResult<Position>(
                  resultStatus: ResultStatus.Error,
                  message: $"Malesef bir sorun oluştu...",
                  exception: ex,
                  data: null
                  );
            }
        }

        public async Task<IResult> Delete(int id)
        {
            var position = await _unitOfWork.Positions.GetAsync(p => p.Id == id);
            if (position != null)
            {
                position.DeletedDate = DateTime.UtcNow;
                position.UpdatedDate = DateTime.UtcNow;
                await _unitOfWork.Positions.UpdateAsync(position);
                await _unitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, "Pozisyon başarıyla silindi.");
            }
            return new Result(ResultStatus.Error, "Pozisyon bulunamadı.");
        }

        public async Task<IDataResult<IList<Position>>> GetAll()
        {
            var positions = await _unitOfWork.Positions.GetAllAsync(p => !p.DeletedDate.HasValue, q => q.OrderByDescending(p => p.UpdatedDate));
            if (positions.Count > 0)
            {
                return new DataResult<IList<Position>>(ResultStatus.Success, positions);
            }
            return new DataResult<IList<Position>>(ResultStatus.Error, "Hiç pozisyon bulunamadı.", null);
        }

        public async Task<IDataResult<PageResponse<Position>>> GetToGrid(PageRequest request)
        {
            try
            {
                Expression<Func<Position, bool>> predicate = x => !x.DeletedDate.HasValue;
                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<Position>(request.Filter, deleted: false);

                var positions = await _unitOfWork.Positions.GetEntitiesWithPaginationAsync(
                    request.PageIndex,
                    request.PageSize,
                    predicate: predicate,
                    orderBy: q => q.OrderByDescending(p => p.UpdatedDate)
                );

                if (positions.Items.Any())
                {
                    return new DataResult<PageResponse<Position>>(ResultStatus.Success, positions);
                }
                return new DataResult<PageResponse<Position>>(ResultStatus.Error, "Hiç pozisyon bulunamadı.", null);
            }
            catch (Exception ex)
            {
                return new DataResult<PageResponse<Position>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }

        public async Task<IDataResult<Position>> GetById(int id)
        {
            var position = await _unitOfWork.Positions.GetAsync(p => p.Id == id);
            if (position != null)
            {
                return new DataResult<Position>(ResultStatus.Success, position);
            }
            return new DataResult<Position>(ResultStatus.Error, "Pozisyon bulunamadı.", null);
        }

        private string GenerateUniquePositionCode(string name)
        {
            var shortName = name.Length >= 5 ? name.Substring(0, 5).ToUpper() : name.ToUpper();
            var randomString = Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper();
            return $"{shortName}-{randomString}";
        }
       
    }
}