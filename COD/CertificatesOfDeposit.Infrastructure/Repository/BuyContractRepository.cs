using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IBuyContractRepository : IGenericRepository<BuyContract>
    {
        Task<int> ApproveAsync(BuyContract entity);
    }
    public class BuyContractRepository : IBuyContractRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public BuyContractRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public async Task<int> AddAsync(BuyContract entity)
        {
            var sql = @"insert into tkgtcg_buy_contract
                      (buy_id,
                       status,
                       buy_file,
                       created_user,
                       created_at,
                       file_name,file_type)
                    values
                      (:v_buy_id,
                       :v_status,
                       :v_buy_file,
                       :v_created_user,
                       :v_created_at,
                       :v_file_name,
                       :v_file_type)";
            var result = await this._context.Connection.ExecuteAsync(sql, new {
                v_buy_id = entity.BuyID,
                v_status= entity.Status,
                v_buy_file = entity.BuyFile,
                v_created_user = entity.CreatedUser,
                v_created_at = entity.CreatedAt,
                v_file_name = entity.FileName,
                v_file_type = entity.FileType

            });
            return result;
        }

        public Task<int> ApproveAsync(BuyContract entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(string id)
        {
            var sql = @"delete from tkgtcg_buy_contract
                        where buy_id = :v_buy_id";
            var result = await this._context.Connection.ExecuteAsync(sql, new { v_buy_id = id });
            return result;
        }

        public Task<IReadOnlyList<BuyContract>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BuyContract> GetByIdAsync(string id)
        {
            var sql = @"select buy_id,
                           status,
                           buy_file,
                           created_user,
                           created_at,
                           updated_user,
                           updated_at,
                           approved_user,
                           approved_at,
                           file_name,file_type
                      from tkgtcg_buy_contract where buy_id = :v_buy_id";
            var result = await this._context.Connection.QuerySingleOrDefaultAsync<BuyContract>(sql, new { v_buy_id = id });
            return result;
        }

        public async Task<int> UpdateAsync(BuyContract entity)
        {
            var sql = @"update tkgtcg_buy_contract
                           set
                               status = :v_status,
                               buy_file = :v_buy_file,
                               approved_user = :v_approved_user,
                               approved_at = :v_approved_at,
                               updated_user = :v_updated_user,
                               updated_at = :v_updated_at,
                               file_name = :v_file_name,
                               file_type = :v_file_type
                         where buy_id = :v_buy_id";
            var result = await this._context.Connection.ExecuteAsync(sql, new 
            { 
                v_status = entity.Status,
                v_buy_file = entity.BuyFile, 
                v_approved_user  = entity.ApprovedUser, 
                v_approved_at = entity.ApprovedAt,
                v_updated_user = entity.UpdatedUser,
                v_updated_at = entity.UpdatedAt,
                v_file_name = entity.FileName,
                v_file_type = entity.FileType,
                v_buy_id = entity.BuyID 
            });
            return result;
        }
    }
}
