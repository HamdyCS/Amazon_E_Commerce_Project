using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IApplicationTypeRepository : IGenericRepository<ApplicationType>
    {
        public Task<ApplicationType> GetByEnApplicationTypeAsync(EnApplicationType enApplicationType);
    }
}
