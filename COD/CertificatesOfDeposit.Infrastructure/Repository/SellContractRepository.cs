using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface ISellContractRepository : IGenericRepository<SellContract>
    {
        Task<int> ApproveAsync(SellContract entity);
    }
    public class SellContractRepository : ISellContractRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public SellContractRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public async Task<int> AddAsync(SellContract entity)
        {
            var sql = @"insert into tkgtcg_sell_contract
                      (sell_id,
                       status,
                       sell_file,
                       created_user,
                       created_at,
                       file_name,
                       file_type)
                    values
                      (:v_sell_id,
                       :v_status,
                       :v_sell_file,
                       :v_created_user,
                       :v_created_at,
                       :v_file_name,
                       :v_file_type)";
            var result = await this._context.Connection.ExecuteAsync(sql, new
            {
                v_sell_id = entity.SellID,
                v_status = entity.Status,
                v_sell_file = entity.SellFile,
                v_created_user = entity.CreatedUser,
                v_created_at = entity.CreatedAt,
                v_file_name = entity.FileName,
                v_file_type = entity.FileType

            });
            return result;
        }

        public Task<int> ApproveAsync(SellContract entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(string id)
        {
            var sql = @"delete from tkgtcg_sell_contract
                        where sell_id = :v_sell_id";

            var result = await this._context.Connection.ExecuteAsync(sql, new { v_sell_id = id });
            return result;
        }

        public Task<IReadOnlyList<SellContract>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<SellContract> GetByIdAsync(string id)
        {
            var sql = @"select sell_id,
                           status,
                           sell_file,
                           created_user,
                           created_at,
                           updated_user,
                           updated_at,
                           approved_user,
                           approved_at,
                           file_name,file_type
                      from tkgtcg_sell_contract where sell_id = :v_sell_id";
            var result = await this._context.Connection.QuerySingleOrDefaultAsync<SellContract>(sql, new { v_sell_id = id });
            return result;
        }

        public async Task<int> UpdateAsync(SellContract entity)
        {
            var sql = @"update tkgtcg_sell_contract
                           set
                               status = :v_status,
                               sell_file = :v_sell_file,
                               approved_user = :v_approved_user,
                               approved_at = :v_approved_at,
                               updated_user = :v_updated_user,
                               updated_at = :v_updated_at,
                               file_name = :v_file_name,
                               file_type = :v_file_type
                         where sell_id = :v_sell_id";
            var result = await this._context.Connection.ExecuteAsync(sql, new 
            { 
                v_status = entity.Status,
                v_sell_file = entity.SellFile,
                v_approved_user = entity.ApprovedUser,
                v_approved_at = entity.ApprovedAt,
                v_updated_user = entity.UpdatedUser,
                v_updated_at = entity.UpdatedAt,
                v_file_name = entity.FileName,
                v_file_type = entity.FileType,
                v_sell_id = entity.SellID 
            });
            return result;
        }
    }
}
