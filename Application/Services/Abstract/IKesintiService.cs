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
    public interface IKesintiService
    {
        Task<IDataResult<IList<Kesinti>>> GetAll();

        Task<IDataResult<PageResponse<Kesinti>>> GetToGrid(PageRequest request);

        Task<IDataResult<Kesinti>> Edit(Kesinti kesinti);

        Task<IResult> Delete(int Id);

        Task<IDataResult<Kesinti>> GetById(int id);
    }
}