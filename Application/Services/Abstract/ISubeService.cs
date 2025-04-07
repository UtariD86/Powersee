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

    public interface ISubeService
    {

        Task<IDataResult<IList<Sube>>> GetAll();

        Task<IDataResult<PageResponse<Sube>>> GetToGrid(PageRequest request);

        Task<IDataResult<Sube>> Edit(Sube sube);

        Task<IResult> Delete(int Id);

 
        Task<IDataResult<Sube>> GetById(int id);
    }
}
