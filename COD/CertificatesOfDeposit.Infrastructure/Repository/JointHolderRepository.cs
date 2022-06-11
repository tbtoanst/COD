using Microsoft.Extensions.Configuration;
using Dapper;
using CertificatesOfDeposit.Infrastructure.Interfaces;
using CertificatesOfDeposit.Core.Entities;
using Dapper.Oracle;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure.Repository
{
    public interface IJointHolderRepository : IGenericRepository<JointHolder>
    {
        Task<IReadOnlyList<JointHolder>> GetAllAsync(string accNum);

    }
    public class JointHolderRepository : IJointHolderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionContext _context;
        public JointHolderRepository(IConfiguration configuration, IConnectionContext context)
        {
            this._configuration = configuration;
            this._context = context;
        }


        public Task<int> AddAsync(JointHolder entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<JointHolder>> GetAllAsync(string accNum)
        {
            var paramQuery = new OracleDynamicParameters();
            paramQuery.Add(name: "V_ACC_NUM", accNum, dbType: OracleMappingType.NVarchar2, direction: ParameterDirection.Input);
            paramQuery.Add(name: "P_DATA", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
            var queryMultiDatas = await this._context.Connection.QueryMultipleAsync("TKGTCG_PUSH_CORE_PKG.GET_ALL_JOINT_HOLDER", param: paramQuery, commandType: CommandType.StoredProcedure);
            return queryMultiDatas.Read<JointHolder>().ToList();
        }

        public Task<IReadOnlyList<JointHolder>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(JointHolder entity)
        {
            throw new NotImplementedException();
        }

        Task<JointHolder> IGenericRepository<JointHolder>.GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
