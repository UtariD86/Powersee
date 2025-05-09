﻿using Core.Dtos.Abstract;
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
   
    public interface IPositionService
    {
        
        Task<IDataResult<IList<Position>>> GetAll();

       
        Task<IDataResult<PageResponse<PositionDetailsDto>>> GetToGrid(PageRequest request);

        Task<IDataResult<Position>> Edit(Position position);

        Task<IResult> Delete(int Id);

      
        
        Task<IDataResult<Position>> GetById(int id);

        Task<List<BirimSayiDto>> GetAllPositionsWithPersonelCounts();
    }
}
