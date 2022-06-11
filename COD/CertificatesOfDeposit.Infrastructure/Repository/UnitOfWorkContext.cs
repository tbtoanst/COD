using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public class UnitOfWorkContext : DbConnectionRepositoryBase, IUnitOfWorkContext, IConnectionContext
    {
        private UnitOfWork _unitOfWork;

        private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

        public IDbConnection Connection => IsUnitOfWorkOpen ? _unitOfWork.Connection : _conn;

        public UnitOfWorkContext(IDbConnectionFactory dbConnectionFactory): base(dbConnectionFactory)
        {
        }

        public UnitOfWork Create()
        {
            if (IsUnitOfWorkOpen)
            {
                throw new InvalidOperationException(
                    "Cannot begin a transaction before the unit of work from the last one is disposed");
            }

            _unitOfWork = new UnitOfWork(_conn);
            return _unitOfWork;
        }
    }

    public interface IConnectionContext
    {
        IDbConnection Connection {
            get;
        } 
    }

    public interface IUnitOfWorkContext
    {
        UnitOfWork Create();
    }
}
