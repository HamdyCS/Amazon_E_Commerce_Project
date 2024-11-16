using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class OtpRepository : GenericRepository<Otp>, IOtpRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OtpRepository> _logger;

        public OtpRepository(AppDbContext context, ILogger<OtpRepository> logger) : base(context, logger, "Otps")
        {
            _context = context;
            _logger = logger;
        }

       

        public async Task<bool> CheckIsOtpActiveByEmailAndCodeAsync(string Email, string Code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Code, nameof(Code));

            try
            {
                var otp = await _context.Otps.Where(x=>x.Email==Email).OrderBy(x=>x.CreatedAt).LastOrDefaultAsync(o => o.Code == Code && o.IsActive);
                return otp != null;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<bool> CheckIsOtpUsedByEmailAndCodeAsync(string Email, string Code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Code, nameof(Code));


            try
            {
                var otp = await _context.Otps.Where(x => x.Email == Email).OrderBy(x => x.CreatedAt).LastOrDefaultAsync( o=> o.Code == Code && o.IsUsed);
                return otp != null;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<Otp> GetTheLastByEmailAndCodeAsync(string Email, string Code)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Email, nameof(Email));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(Code, nameof(Code));

            try
            {
                var otp = await _context.Otps.Where(x => x.Email == Email).OrderBy(x => x.CreatedAt).LastOrDefaultAsync(o => o.Code == Code);
                return otp;
            }
            catch(Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
