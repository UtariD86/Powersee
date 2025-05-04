using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstract
{

    public interface IPlanlanmisVardiyaService
    {

        Task<IDataResult<IList<PlanlanmisVardiya>>> GetAll();

        Task<IDataResult<PageResponse<PlanlanmisVardiya>>> GetToGrid(PageRequest request);

        Task<IDataResult<PlanlanmisVardiya>> Edit(PlanlanmisVardiya planlanmisvardiya);

        Task<IResult> Delete(int Id);


        Task<IDataResult<PlanlanmisVardiya>> GetById(int id);
    }
}
