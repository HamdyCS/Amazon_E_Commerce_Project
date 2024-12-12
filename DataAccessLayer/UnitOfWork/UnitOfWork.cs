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


        public ICityRepository cityRepository { get; private set; }
        public IPersonRepository personRepository { get; private set; }
        public IRefreshTokenRepository refreshTokenRepository { get; private set; }
        public IRoleManagerRepository roleManagerRepository { get; private set; }
        public IUserAdderssRepository userAdderssRepository { get; private set; }
        public IUserRepository userRepository { get; private set; }

        public IOtpRepository otpRepository { get; private set; }
        public IProductCategoryImageRepository productCategoryImageRepository { get; private set; }

        public IProductCategoryRepository productCategoryRepository {  get; private set; }

        public IBrandRepository brandRepository { get; private set; }

      

        public UnitOfWork(AppDbContext context,ILogger<UnitOfWork> logger,
            ICityRepository cityRepository,IPersonRepository personRepository,
            IRefreshTokenRepository refreshTokenRepository,IRoleManagerRepository roleManagerRepository,
            IUserAdderssRepository userAdderssRepository,IUserRepository userRepository, IOtpRepository otpRepository,
            IProductCategoryImageRepository productCategoryImageRepository, 
            IProductCategoryRepository productCategoryRepository, IBrandRepository brandRepository
            ) 
        {
            this._context = context;
            this._logger = logger;
            this.cityRepository = cityRepository;
            this.personRepository = personRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.roleManagerRepository = roleManagerRepository;
            this.userAdderssRepository = userAdderssRepository;
            this.userRepository = userRepository;
            this.otpRepository = otpRepository;
            this.productCategoryImageRepository = productCategoryImageRepository;
            this.productCategoryRepository = productCategoryRepository;
            this.brandRepository = brandRepository;

        }

        public async Task BeginTransactionAsync()
        {
            try
            {
                 await _context.Database.BeginTransactionAsync();
                return;
            }
            catch (Exception ex) 
            {
               
                _logger.LogError("Error while Beginning Transaction, Error : {ErrorMessage}", ex.Message);
                throw new Exception($"Error while Beginning Transaction, Error : {ex.Message}");
            }
        }

        public async Task CommitTransactionAsync()
        {
           
            try
            {
                await _context.Database.CommitTransactionAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while Transitioning, Error : {ErrorMessage}", ex.Message);
                throw new Exception($"Error while Transitioning, Error : {ex.Message}");
            }

        }
       
        public async Task<long> CompleteAsync()
        {
            try
            {
                var result =  await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while saving changes to database, Error : {ErrorMessage}", ex.Message);
                throw new Exception($"Error while saving changes to database, Error :  {ex.Message}");
            }
        }

        public async void Dispose()
        {
            try
            {
                await _context.DisposeAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while Disposeing context object, Error : {ErrorMessage}", ex.Message);
                throw new Exception($"Error while Disposeing context object, Error :  {ex.Message}");

            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
               await _context.Database.RollbackTransactionAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while Rolling back Transaction, Error : {Error message}",ex.Message);
                throw new Exception($"Error while Rolling back Transaction, Error : {ex.Message}");
            }
        }
    }
}
