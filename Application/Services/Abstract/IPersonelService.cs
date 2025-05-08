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

    public interface IPersonelService
    {

        Task<IDataResult<IList<Personel>>> GetAll();

        Task<IDataResult<PageResponse<PersonelListDto>>> GetToGrid(PageRequest request);

        Task<IDataResult<Personel>> Edit(Personel personel);

        Task<IResult> Delete(int Id);


        Task<IDataResult<Personel>> GetById(int id);
        Task<IDataResult<Personel>> GetByCode(string code);
    }
}
