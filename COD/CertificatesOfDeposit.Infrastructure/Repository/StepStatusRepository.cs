using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IStepStatusRepository : IGenericRepository<StepStatus>
    {
        Task<List<StepStatus>> GetNextStepAsync(string id, string productCode);
        Task<List<StepStatus>> GetDeleteStepAsync(string id, string productCode);
        Task<List<StepStatus>> GetApproveStepAsync(string id, string productCode);
        Task<List<StepStatus>> GetRejectStepAsync(string id, string productCode);
        Task<List<StepStatus>> GetStartStepAsync(string productCode);
    }

    public class StepStatusRepository : IStepStatusRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;

        public StepStatusRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }

        public async Task<int> AddAsync(StepStatus entity)
        {
            var sql = @"insert into TKGTCG_TRAN_LOG (
                        id
                        ,parent_id
                        ,name
                        ,is_active
                        ,is_delete
                        ,is_reject
                        ,is_approve
                        ,phase_code
                        ,lane_code
                        ,product_code
                        ,code
                        ,created_date
                        ,created_user
                    values (
                        @id
                        ,@parent_id
                        ,@name
                        ,@is_active
                        ,@is_delete
                        ,@is_reject
                        ,@is_approve
                        ,@phase_code
                        ,@lane_code
                        ,@product_code
                        ,sysdate()
                        ,@created_user);";
            var result = await this._context.Connection.ExecuteAsync(sql, entity);
            return result;
        }

        public async Task<int> DeleteAsync(string id)
        {
            var sql = @"UPDATE tkgtcg_step_status
                        SET is_active = 0
                        WHERE id = :v_id";
            var result = await this._context.Connection.ExecuteAsync(sql, new { v_id = id });
            return result;
        }

        public async Task<IReadOnlyList<StepStatus>> GetAllAsync()
        {
            var sql = @"SELECT r.* FROM tkgtcg_step_status r";
            var result = await this._context.Connection.QueryAsync<StepStatus>(sql);
            return result.ToList();
        }

        public async Task<List<StepStatus>> GetApproveStepAsync(string id, string productCode)
        {
            var sql = @"SELECT * FROM tkgtcg_step_status
                        WHERE is_approve = 1 and parent_id = :v_id and product_code = :v_product_code";
            var result = await this._context.Connection.QueryAsync<StepStatus>(sql, new { v_id = id, v_product_code = productCode });
            return result.ToList();
        }        

        public Task<StepStatus> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StepStatus>> GetDeleteStepAsync(string id, string productCode)
        {
            var sql = @"SELECT * FROM tkgtcg_step_status
                        WHERE is_delete = 1 and parent_id = :v_id and product_code = :v_product_code";
            var result = await this._context.Connection.QueryAsync<StepStatus>(sql, new { v_id = id, v_product_code = productCode });
            return result.ToList();
        }

        public async Task<List<StepStatus>> GetNextStepAsync(string id, string productCode)
        {
            var sql = @"SELECT * FROM tkgtcg_step_status
                        WHERE is_delete = 0 
                              and is_reject = 0 
                              and is_approve = 0 
                              and parent_id = :v_id
                              and product_code = :v_product_code";
            var result = await this._context.Connection.QueryAsync<StepStatus>(sql, new { v_id = id, v_product_code = productCode });
            return result.ToList();
        }

        public async Task<List<StepStatus>> GetRejectStepAsync(string id, string productCode)
        {
            var sql = @"SELECT * FROM tkgtcg_step_status
                        WHERE is_reject = 1 and parent_id = :v_id and product_code = :v_product_code";
            var result = await this._context.Connection.QueryAsync<StepStatus>(sql, new { v_id = id, v_product_code = productCode });
            return result.ToList();
        }

        public async Task<List<StepStatus>> GetStartStepAsync(string productCode)
        {
            var sql = @"SELECT * FROM tkgtcg_step_status
                        WHERE parent_id is null and product_code = :v_product_code";
            var result = await this._context.Connection.QueryAsync<StepStatus>(sql, new { v_product_code = productCode });
            return result.ToList();
        }

        public async Task<int> UpdateAsync(StepStatus entity)
        {
            var sql = @"UPDATE tkgtcg_step_status
                        SET 
	                        parent_id = @parent_id
                            ,name = @name
                            ,is_active = @is_active
                            ,is_delete = @is_delete
                            ,is_reject = @is_reject
                            ,is_approve = @is_approve
                            ,phase_code = @phase_code
                            ,lane_code = @lane_code
                            ,product_code = @product_code
                            ,code = @code
                            ,updated_by = @updated_by
                            ,updated_on = SYSDATE()
                        WHERE 
	                        id = @id";
            var result = await this._context.Connection.ExecuteAsync(sql, entity);
            return result;
        }
    }
}
