using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly IDbTransaction _transaction;
        public IDbConnection Connection { get; }

        public bool IsDisposed { get; private set; } = false;

        public UnitOfWork(IDbConnection connection)
        {
            Connection = connection;
            Connection.Open();
            _transaction = Connection.BeginTransaction();
        }

        public async Task RollBackAsync()
        {
            _transaction.Rollback();
        }

        public async Task CommitAsync()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            Connection.Close();
            Connection.Dispose();
            IsDisposed = true;
        }
    }
}
