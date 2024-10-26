using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CityRepository> _logger;
        private readonly string _TableName = "Cites";
        public CityRepository(AppDbContext context, ILogger<CityRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

    }
}
