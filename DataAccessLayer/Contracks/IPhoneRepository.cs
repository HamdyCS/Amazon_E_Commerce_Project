using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IPhoneRepository : IGenericRepository<Phone>
    {
        
        Task<IEnumerable<Phone>> GetAllPersonPhonesByUserIdAsync();

        Task<IEnumerable<Phone>> GetAllPersonPhonesByUserEmailAsync();

        void DeletePersonPhoneByUserEmail();

       
    }
}
