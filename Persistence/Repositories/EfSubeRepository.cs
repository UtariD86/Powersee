using Core.Data.Repositories.Concrete;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Abstract;
using Persistence.Concrete;
using Persistence.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class EfSubeRepository : EfEntityRepositoryBase<Sube>, ISubeRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly ServiceProvider serviceProvider;

        public EfSubeRepository(ApplicationDbContext _context, IUnitOfWork unitOfWork) : base(_context)
        {
            context = _context;
            this.unitOfWork = unitOfWork;
        }
    }
}
