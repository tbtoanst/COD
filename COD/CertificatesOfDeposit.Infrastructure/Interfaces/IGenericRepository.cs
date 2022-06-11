using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Infrastructure.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Lấy thông tin chi tiết theo id
        /// </summary>
        /// <param name="id">id chi tiết</param>
        /// <returns></returns>
        Task<T> GetByIdAsync(string id);

        /// <summary>
        /// Lấy tất cả danh sách
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="entity">data input</param>
        /// <returns></returns>
        Task<int> AddAsync(T entity);

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="entity">data input</param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity);

        /// <summary>
        /// Xóa thông tin theo id
        /// </summary>
        /// <param name="id">id data info</param>
        /// <returns></returns>
        Task<int> DeleteAsync(string id);
    }
}
