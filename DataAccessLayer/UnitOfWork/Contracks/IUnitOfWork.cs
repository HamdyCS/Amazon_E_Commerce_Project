using DataAccessLayer.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork.Contracks
{
    public interface IUnitOfWork : IDisposable
    {
        public ICityRepository cityRepository { get; }
        public IPersonRepository personRepository { get; }
        public IRefreshTokenRepository refreshTokenRepository { get; }
        public IRoleManagerRepository roleManagerRepository { get; }
        public IUserAdderssRepository userAdderssRepository { get; }
        public IUserRepository userRepository { get; }
        public Task<long> CompleteAsync();

        public Task BeginTransactionAsync();

        public Task CommitTransactionAsync();

        public Task RollbackTransactionAsync();

    }
}
