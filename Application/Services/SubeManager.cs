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

    public class SubeManager : ISubeService
    {

        private readonly IUnitOfWork _unitOfWork;


        private readonly FilterHelper _filterHelper;


        public SubeManager(IUnitOfWork unitOfWork, FilterHelper filterHelper)
        {

            _unitOfWork = unitOfWork;
            _filterHelper = filterHelper;


        }


        public async Task<IDataResult<Sube>> Edit(Sube sube)
        {

            try
            {

                if (sube != null && sube.Id != 0)
                {

                    sube.UpdatedDate = DateTime.UtcNow;

                    await _unitOfWork.Subeler.UpdateAsync(sube);


                    await _unitOfWork.SaveChangesAsync();


                    return new DataResult<Sube>(
                        data: sube,
                        resultStatus: ResultStatus.Success,
                        message: $"{sube.Subeisim} isimli şube başarıyla güncellendi."
                        );
                }
                else
                {

                    sube.UpdatedDate = DateTime.UtcNow;
                    sube.CreatedDate = DateTime.UtcNow;


                    await _unitOfWork.Subeler.AddAsync(sube);

         
                    await _unitOfWork.SaveChangesAsync();

                    return new DataResult<Sube>(ResultStatus.Success, sube.Subeisim + "Başarıyla Eklendi", sube);
                }
            }
            catch (Exception ex)
            {
        
                return new DataResult<Sube>(
                   resultStatus: ResultStatus.Error,
                   message: $"Malesef bir sorun oluştu...",
                   exception: ex,
                   data: null
                   );
            }
        }

      
        public async Task<IResult> Delete(int Id)
        {

            var _sube = _unitOfWork.Subeler.Get(Id);

            if (_sube != null)
            {

                _sube.DeletedDate = DateTime.UtcNow;

               
                _sube.UpdatedDate = DateTime.UtcNow;

           
                await _unitOfWork.Subeler.UpdateAsync(_sube);

                
                await _unitOfWork.SaveChangesAsync();

               
                return new Result(ResultStatus.Success, $"{_sube.Subeisim} Başarıyla Silindi");
            }
      
            return new Result(ResultStatus.Error, "Seçili şube bulunamadı");
        }


        public async Task<IDataResult<IList<Sube>>> GetAll()
        {
           
            var subeler = await _unitOfWork.Subeler.GetAllAsync(
                predicate: d => !d.DeletedDate.HasValue,
                orderBy: q => q.OrderByDescending(d => d.UpdatedDate)
            );

            
            if (subeler.Count > -1)
            {
                
                return new DataResult<IList<Sube>>(ResultStatus.Success, subeler);
            }
           
            return new DataResult<IList<Sube>>(ResultStatus.Error, "Hiç Şube bulunamadı", null);
        }

        
        public async Task<IDataResult<PageResponse<Sube>>> GetToGrid(PageRequest request)
        {
            try
            {
                
                Expression<Func<Sube, bool>> predicate = x => !x.DeletedDate.HasValue;
                if (!string.IsNullOrEmpty(request.Filter))
                    predicate = _filterHelper.GetExpression<Sube>(request.Filter, deleted: false);

               
                var subeler = await _unitOfWork.Subeler.GetEntitiesWithPaginationAsync(
                    request.PageIndex, 
                    request.PageSize,  
                    predicate: predicate, 
                    orderBy: q => q.OrderByDescending(d => d.UpdatedDate) 
                );

               
                if (subeler.Items.Any())
                {
                    return new DataResult<PageResponse<Sube>>(ResultStatus.Success, subeler);
                }

                
                return new DataResult<PageResponse<Sube>>(ResultStatus.Error, "Hiç Şube bulunamadı", null);
            }
            catch (Exception ex)
            {
                
                return new DataResult<PageResponse<Sube>>(ResultStatus.Error, $"Bir hata oluştu: {ex.Message}", null);
            }
        }



       
        public async Task<IDataResult<Sube>> GetById(int id)
        {
        
            var sube = _unitOfWork.Subeler.Get(id);

            
            if (sube != null)
            {
        
                return new DataResult<Sube>(ResultStatus.Success, sube);
            }
          
            return new DataResult<Sube>(ResultStatus.Error, "Şube bulunamadı", null);
        }

    }
}
