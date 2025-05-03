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
    public interface ITalepService
    {
        Task<IDataResult<IList<Talep>>> GetAll();

        /// <summary>
        /// Departmanları belli bir aralıkta getirir
        /// </summary>
        Task<IDataResult<PageResponse<TalepDetailsDto>>> GetToGrid(PageRequest request);

        /// <summary>
        /// Departman ekleme ve güncelleme işlemleri
        /// </summary>
        Task<IDataResult<Talep>> Edit(Talep talep);

        /// <summary>
        /// Departman silme işlemi
        /// </summary>
        Task<IResult> Delete(int Id);

        /// <summary>
        /// Departman id sine göre getirme işlemi
        /// </summary>
        Task<IDataResult<Talep>> GetById(int id);
    }
}
