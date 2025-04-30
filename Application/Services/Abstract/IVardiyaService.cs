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

    public interface IVardiyaService
    {

        Task<IDataResult<IList<Vardiya>>> GetAll();

        Task<IDataResult<PageResponse<Vardiya>>> GetToGrid(PageRequest request);

        Task<IDataResult<Vardiya>> Edit(Vardiya vardiya);

        Task<IResult> Delete(int Id);


        Task<IDataResult<Vardiya>> GetById(int id);
    }
}
