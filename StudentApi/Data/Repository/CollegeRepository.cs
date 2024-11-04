using Microsoft.EntityFrameworkCore;
using StudentApi.Controllers;
using System.Linq.Expressions;

namespace StudentApi.Data.Repository
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        private readonly StudentDbContext _studentDbContext;
        private DbSet<T> _dbSet;
        public CollegeRepository(StudentDbContext studentDbContext) 
        {
            _studentDbContext = studentDbContext;
            _dbSet = _studentDbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T dbRecord)
        {
            _studentDbContext.Add(dbRecord);
            await _studentDbContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<bool> DeleteAsync(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _studentDbContext.SaveChangesAsync();
            return true;
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T,bool>> filter, bool useNoTracking = false)
        {
            if (useNoTracking)
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public Task<T> GetByIdAsync(object id, bool v)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T> UpdateAsync(T dbRecord)
        {
            _studentDbContext.Update(dbRecord);
            await _studentDbContext.SaveChangesAsync();
            return dbRecord;

        }



    }
}
