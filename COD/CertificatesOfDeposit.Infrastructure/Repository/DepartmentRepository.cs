using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        /// <summary>
        /// Lấy danh sách Sản phẩm
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task<PagedResults<Department>> QueryAsync(int pageNum = 0, int pageSize = int.MaxValue, int isActive = 1);

        /// <summary>
        /// Lấy danh sách sản phẩm theo người dùng
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Department>> QueryByParentAsync(string parentId);

        /// <summary>
        /// Kiểm tra tồn tại mã code
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        Task<bool> CheckExistsByCode(string deptCode);

        /// <summary>
        /// Lấy danh sách phòng ban theo user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Department>> GetAllByUserIdAsync(string userId);
    }
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public DepartmentRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }
        public async Task<int> AddAsync(Department entity)
        {
            //var sql = @"INSERT INTO org_departments 
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
            //            FROM org_departments a
            //            WHERE a.code = @code ";
            //var deptCodeExists = await this._context.Connection.ExecuteScalarAsync<bool>(sql,
            //    new { code = deptCode });
            //return deptCodeExists;
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(string id)
        {
            //var sql = @"DELETE FROM org_departments
            //            WHERE 
	           //             id = @id";

            //var result = await this._context.Connection.ExecuteAsync(sql, new { Id = id });
            //return result;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Department>> GetAllAsync()
        {
            //var sql = @"SELECT o.* FROM org_departments o";
            //var result = await this._context.Connection.QueryAsync<Department>(sql);
            //return result.ToList();
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Department>> GetAllByUserIdAsync(string userId)
        {
            var sql = @"SELECT a.* FROM org_department a join org_user b on b.department_id = a.id where b.id = :v_user_id or b.user_name = :v_user_id";
            var result = await this._context.Connection.QueryAsync<Department>(sql, new
            {
                v_user_id = userId
            });
            return result.ToList();
        }

        public async Task<Department> GetByIdAsync(string id)
        {
            //var sql = @"SELECT o.*
            //            FROM org_departments o
            //            WHERE o.id = @id";
            //var result = await this._context.Connection.QuerySingleOrDefaultAsync<Department>(sql, new { Id = id });
            //return result;
            throw new NotImplementedException();
        }

        public async Task<PagedResults<Department>> QueryAsync(int pageNum = 0, int pageSize = int.MaxValue, int isActive = 1)
        {
            //var results = new PagedResults<Department>();

            //var sql = @"SELECT p.*
            //            FROM org_departments p
            //            WHERE p.is_active = @is_active
            //            LIMIT @page_size OFFSET @page_num; 

            //            SELECT 
            //            COUNT(*)
            //            FROM org_departments p
            //            WHERE p.is_active = @is_active";

            //var queryMultiDatas = await this._context.Connection.QueryMultipleAsync(sql,
            //    new
            //    {
            //        is_active = isActive,
            //        page_size = pageSize,
            //        page_num = pageNum * pageSize
            //    });
            //results.Items = queryMultiDatas.Read<Department>().ToList();
            //results.TotalCount = queryMultiDatas.ReadFirst<int>();
            //return results;
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Department>> QueryByParentAsync(string parentId)
        {
            //var sql = @"SELECT o.*
            //            FROM org_departments o
            //            WHERE o.parent_id = @parent_id";
            //var queryDatas = await this._context.Connection.QueryAsync<Department>(sql, new { parent_id = parentId });
            //return queryDatas.ToList();
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(Department entity)
        {
            //var sql = @"UPDATE org_departments
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
