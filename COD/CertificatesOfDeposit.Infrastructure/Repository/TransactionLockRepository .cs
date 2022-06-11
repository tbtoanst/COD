using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface ITransactionLockRepository : IGenericRepository<TransactionLock>
    {
        Task<string> GetStatusLock(string id);
        Task<int> UpdateTransactionLock(string id, string userUpdate, string status);
    }

    public class TransactionLockRepository : ITransactionLockRepository
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IConnectionContext _context;
        
        public TransactionLockRepository(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IConnectionContext context)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._context = context;
        }

        public Task<int> AddAsync(TransactionLock entity)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetStatusLock(string id)
        {
            var sql = @"select status from TKGTCG_LOCK_TRAN t where id = :v_id ";
            var result = await this._context.Connection.ExecuteScalarAsync<string>(sql, new { v_id = id});
            return result;
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TransactionLock>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TransactionLock> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(TransactionLock entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateTransactionLock(string id, string userUpdate, string status)
        {
            var sql = @"update TKGTCG_LOCK_TRAN
                        set 
                            status = :v_status
                            ,updated_user = :v_userUpdate
                            ,updated_date = sysdate
                        where id = :v_id";

            var result = await this._context.Connection.ExecuteAsync(sql, new { v_id = id, v_userUpdate = userUpdate, v_status = status });
            return result;
        }
    }
}
