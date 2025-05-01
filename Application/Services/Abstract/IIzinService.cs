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
    public interface IIzinService
    {

        Task<IDataResult<IList<Izin>>> GetAll();

        Task<IDataResult<PageResponse<IzinListDto>>> GetToGrid(PageRequest request);

        Task<IDataResult<Izin>> Edit(Izin izin);

        Task<IResult> Delete(int Id);


        Task<IDataResult<Izin>> GetById(int id);
    }
}
