using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork.Contracks
{
    public interface IUnitOfWork : IDisposable
    {

        public Task<long> CompleteAsync();

        public Task BeginTransactionAsync();

        public void CommitTransactionAsync();

        public Task RollbackTransactionAsync();

    }
}
