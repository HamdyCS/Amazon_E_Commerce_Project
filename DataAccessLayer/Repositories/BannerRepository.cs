using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace DataAccessLayer.Repositories
{
    public class BannerRepository : GenericRepository<Banner>, IBannerRepository
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _context;

        public BannerRepository(AppDbContext context, ILogger<GenericRepository<Banner>> logger) : base(context, logger, "Banners")
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Banner>> GetActiveBannersOrderByDisplayOrderAsc()
        {
            try
            {
                var activeBanners = await _context.Banners.
                    Where(x => x.IsActive && x.EndDate > DateTime.UtcNow && x.StartDate <= DateTime.UtcNow)
                    .OrderBy(x => x.DisplayOrder).ToListAsync();


                return activeBanners;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);

            }
        }

        public async Task<IEnumerable<Banner>> GetOverLappingBannersOrderByDisplayOrderAsc(DateTime startDate, DateTime endDate)
        {
            ParamaterException.CheckIfObjectIfNotNull(startDate, nameof(startDate));
            ParamaterException.CheckIfObjectIfNotNull(endDate, nameof(endDate));


            try
            {
                //start => 14/3/2025
                //end => 20/3/2025

                //exb
                //16/3/2025  18/3/2025

                //16/3/2025 <= (20/3/2025) endDate 
                //18/3/2025 >= (14/3/2025) startDate
                var overLappingBanners = await _context.Banners.
                    Where(b => b.StartDate <= endDate && b.EndDate >= startDate).AsNoTracking()
                    .OrderBy(x => x.DisplayOrder).ToListAsync();


                return overLappingBanners;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);

            }
        }
    }
}
