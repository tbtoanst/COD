using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface ITransactionLogRepository : IGenericRepository<TransactionLog>
    {
    }

    public class TransactionLogRepository : ITransactionLogRepository
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWorkContext _unitOfWork;
        private readonly IConnectionContext _context;

        public TransactionLogRepository(IConfiguration configuration, IUnitOfWorkContext unitOfWork, IConnectionContext context)
        {
            this._config = configuration;
            this._unitOfWork = unitOfWork;
            this._context = context;
        }

        public async Task<int> AddAsync(TransactionLog entity)
        {
            var sql = @"insert into tkgtcg_tran_log
                      (id, root_id, type, status, created_at, created_user, data)
                    values
                      (:v_id, :v_root_id, :v_type, :v_status, :v_created_at, :v_created_user, :v_data)";
            var result = await this._context.Connection.ExecuteAsync(sql, new
            {
                v_id = entity.Id,
                v_root_id = entity.RootId,
                v_type = entity.Type,
                v_status = entity.Sattus,
                v_created_at = entity.CreatedAt,
                v_created_user = entity.CreatedUser,
                v_data = entity.Data
            });
            return result;
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TransactionLog>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TransactionLog> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(TransactionLog entity)
        {
            throw new NotImplementedException();
        }
    }
}
