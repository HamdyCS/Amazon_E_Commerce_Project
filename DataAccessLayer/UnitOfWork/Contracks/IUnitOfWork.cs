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
        public IPersonRepository personRepository { get; }
<<<<<<< HEAD

        public IUserRepository userRepository { get; }
=======
>>>>>>> cc8c5f0f0dc7b9ce001b93e674c52e553a9adc0b
        public Task<long> CompleteAsync();

        public Task BeginTransactionAsync();

        public void CommitTransactionAsync();

        public Task RollbackTransactionAsync();

    }
}
