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
       Task<Phone> GetPhoneAsTrackingByUserEmailAsync(string email);
       Task<Phone> GetPhoneAsNoTrackingByUserEmailAsync(string email);
       Task<Phone> GetPhoneAsTrackingByUserIdAsync(string UserId);
       Task<Phone> GetPhoneAsNoTrackingByUserIdAsync(string UserId);




    }
}
