using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        private IDbContextTransaction _transaction;

        public ICityRepository cityRepository { get; private set; }
        public IPersonRepository personRepository { get; private set; }
        public IPhoneRepository phoneRepository { get; private set; }
        public IRefreshTokenRepository refreshTokenRepository { get; private set; }
        public IRoleManagerRepository roleManagerRepository { get; private set; }
        public IUserAdderssRepository userAdderssRepository { get; private set; }
        public IUserRepository userRepository { get; private set; }

       

       
        public UnitOfWork(AppDbContext context,ILogger<UnitOfWork> logger,
            ICityRepository cityRepository,IPersonRepository personRepository,IPhoneRepository phoneRepository,
            IRefreshTokenRepository refreshTokenRepository,IRoleManagerRepository roleManagerRepository,
            IUserAdderssRepository userAdderssRepository,IUserRepository userRepository) 
        {
            _context = context;
            this._logger = logger;
            this.cityRepository = cityRepository;
            this.personRepository = personRepository;
            this.phoneRepository = phoneRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.roleManagerRepository = roleManagerRepository;
            this.userAdderssRepository = userAdderssRepository;
            this.userRepository = userRepository;


        }

        public async Task BeginTransactionAsync()
        {
            try
            {
                 _transaction = await _context.Database.BeginTransactionAsync();
            }
            catch (Exception ex) 
            {
                _logger.LogError("Cannot begin Transaction ");
            }
        }

        public async Task CommitTransactionAsync()
        {
            if(_transaction == null)
            {
                _logger.LogError("Transaction object is null");
                return;
            }

            try
            {
                await _transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot Commit Transaction ");
            }

        }
       
        public async Task<long> CompleteAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
           await _context.DisposeAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                _logger.LogError("Transaction object is null");
                return;
            }

            try
            {
               await _transaction.RollbackAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot Roll back Transaction ");
            }
        }
    }
}
