using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contracks
{
    public interface IOtpRepository : IGenericRepository<Otp>
    {
        Task<bool> CheckIsOtpActiveByEmailAndCodeAsync(string Email,string Code);

        Task<bool> CheckIsOtpUsedByEmailAndCodeAsync(string Email, string Code);


        Task<Otp> GetTheLastByEmailAndCodeAsync(string Email, string Code);
    }
}
