using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        /// <summary>
        /// Lấy danh sách chi nhánh
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task<PagedResults<Branch>> QueryAsync(int pageNum = 0, int pageSize = int.MaxValue, int isActive = 1);

        /// <summary>
        /// Lấy danh sách chi nhánh theo parent id
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Branch>> QueryByParentAsync(string parentId);

        /// <summary>
        /// Kiểm tra tồn tại mã code
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        Task<bool> CheckExistsByCode(string deptCode);

        /// <summary>
        /// Lấy danh sách chi nhánh theo user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Branch>> GetAllByUserIdAsync(string userId);
        Task<Branch> GetByBranchCodeAsync(string branchCode);
    }
    public class BranchRepository : IBranchRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public BranchRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }
        public async Task<int> AddAsync(Branch entity)
        {
            //var sql = @"INSERT INTO org_branch 
            //             (id, parent_id, code, name, description, created_at)
            //            VALUES 
            //             (@id, @parent_id, @code, @name, @description, SYSDATE())";

            //var result = await this._context.Connection.ExecuteAsync(sql, entity);
            //return result;
            throw new NotImplementedException();
        }

        public async Task<bool> CheckExistsByCode(string deptCode)
        {
            //var sql = @"SELECT COUNT(*)
            //            FROM org_branch a
            //            WHERE a.code = @code ";
            //var deptCodeExists = await this._context.Connection.ExecuteScalarAsync<bool>(sql,
            //    new { code = deptCode });
            //return deptCodeExists;
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(string id)
        {
            //var sql = @"DELETE FROM org_branch
            //            WHERE 
            //             id = @id";

            //var result = await this._context.Connection.ExecuteAsync(sql, new { Id = id });
            //return result;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Branch>> GetAllAsync()
        {
            var sql = @"SELECT o.*
                        FROM org_branch o where o.active_flag = 1
                        ORDER BY code";
            var result = await this._context.Connection.QueryAsync<Branch>(sql);
            return result.ToList();
        }

        public async Task<IReadOnlyList<Branch>> GetAllByUserIdAsync(string userId)
        {
            var sql = @"SELECT a.* FROM org_branch a join org_user b on b.branch_id = a.id where b.id = :v_user_id or b.user_name = :v_user_id";
            var result = await this._context.Connection.QueryAsync<Branch>(sql, new
            {
                v_user_id = userId
            });
            return result.ToList();
        }

        public async Task<Branch> GetByIdAsync(string id)
        {
            var sql = @"SELECT o.*
                        FROM org_branch o
                        WHERE o.id = :v_id or o.code = :v_id";
            var result = await this._context.Connection.QuerySingleOrDefaultAsync<Branch>(sql, new { v_id = id });
            return result;
        }

        public async Task<Branch> GetByBranchCodeAsync(string branchCode)
        {
            var sql = @"SELECT o.*
                        FROM org_branch o
                        WHERE o.code = :v_code";
            var result = await this._context.Connection.QuerySingleOrDefaultAsync<Branch>(sql, new { v_code = branchCode });
            return result;
        }

        public async Task<PagedResults<Branch>> QueryAsync(int pageNum = 0, int pageSize = int.MaxValue, int isActive = 1)
        {
            //var results = new PagedResults<Branch>();

            //var sql = @"SELECT p.*
            //            FROM org_branch p
            //            WHERE p.is_active = @is_active
            //            LIMIT @page_size OFFSET @page_num; 

            //            SELECT 
            //            COUNT(*)
            //            FROM org_branch p
            //            WHERE p.is_active = @is_active";

            //var queryMultiDatas = await this._context.Connection.QueryMultipleAsync(sql,
            //    new
            //    {
            //        is_active = isActive,
            //        page_size = pageSize,
            //        page_num = pageNum * pageSize
            //    });
            //results.Items = queryMultiDatas.Read<Branch>().ToList();
            //results.TotalCount = queryMultiDatas.ReadFirst<int>();
            //return results;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Branch>> QueryByParentAsync(string parentId)
        {
            //    var sql = @"SELECT o.*
            //                FROM org_branch o
            //                WHERE o.parent_id = @parent_id";
            //    var queryDatas = await this._context.Connection.QueryAsync<Branch>(sql, new { parent_id = parentId });
            //    return queryDatas.ToList();
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(Branch entity)
        {
            //var sql = @"UPDATE org_branch
            //            SET
            //              code = @code,
            //              name = @name,
            //                 parent_id = @parent_id,
            //              description = @description,
            //                 is_active = @is_active,
            //              updated_at = SYSDATE()
            //            WHERE 
            //              id = @id";
            //var result = await this._context.Connection.ExecuteAsync(sql, entity);
            //return result;
            throw new NotImplementedException();
        }
    }
}
