using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstract
{
    public interface IPlanlanmisVardiyaPersonelService
    {
        Task<IDataResult<IList<PlanlanmisVardiyaPersonel>>> GetAll();

        Task<IDataResult<PageResponse<PlanlanmisVardiyaPersonel>>> GetToGrid(PageRequest request);

        Task<IDataResult<PlanlanmisVardiyaPersonel>> Edit(PlanlanmisVardiyaPersonel PlanlanmisVardiyaPersonel);

        Task<IResult> Delete(int Id);

        Task<IDataResult<PlanlanmisVardiyaPersonel>> GetById(int id);

        Task<IResult> UpdateVardiyaPersonel(int vardiyaId, List<int> personelIds);

        Task<List<int>> GetAllPersonelIds(int vardiyaId);

        Task<IDataResult<IList<PlanlanmisVardiyaPersonel>>> TryOperation(bool isGiris, int vardiyaId, int PersonelId);
    }
}
