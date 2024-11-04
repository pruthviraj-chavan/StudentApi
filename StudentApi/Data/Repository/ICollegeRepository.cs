using StudentApi.Controllers;
using System.Linq.Expressions;

namespace StudentApi.Data.Repository
{
    public interface ICollegeRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        Task<T> GetByNameAsync(Expression<Func<T, bool>> filter);

        Task<T> CreateAsync(T dbRecord); // T is written for generic means it is used anywhere in project
        Task<T> UpdateAsync(T dbRecord);

     
       
        Task<bool> DeleteAsync(T dbRecord);
    }
}
