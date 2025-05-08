using Core.Dtos.Abstract;
using Core.Dtos.Concrete;
using Domain.Dtos;
using Domain.Dtos.Report;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstract
{
    /// <summary>
    /// Department servis işlemleri
    /// </summary>
    public interface IDepartmentService
    {
        /// <summary>
        /// Tüm departmanları getirir
        /// </summary>
        Task<IDataResult<IList<Department>>> GetAll();

        /// <summary>
        /// Departmanları belli bir aralıkta getirir
        /// </summary>
        Task<IDataResult<PageResponse<DepartmentListDto>>> GetToGrid(PageRequest request);

        /// <summary>
        /// Departman ekleme ve güncelleme işlemleri
        /// </summary>
        Task<IDataResult<Department>> Edit(Department department);

        /// <summary>
        /// Departman silme işlemi
        /// </summary>
        Task<IResult> Delete(int Id);

        /// <summary>
        /// Departman id sine göre getirme işlemi
        /// </summary>
        Task<IDataResult<Department>> GetById(int id);

        Task<List<BirimSayiDto>> GetAllDepartentsWithPersonelCounts();
    }
}
