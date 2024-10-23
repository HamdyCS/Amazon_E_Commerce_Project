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
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GenericRepository<Person>> _logger;

        public PersonRepository(AppDbContext context, ILogger<GenericRepository<Person>> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
