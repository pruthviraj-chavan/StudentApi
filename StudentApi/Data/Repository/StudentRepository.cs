using Microsoft.EntityFrameworkCore;
using StudentApi.Controllers;

namespace StudentApi.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDbContext _studentDbContext;
        private object studentToDelete;

        public StudentRepository(StudentDbContext studentDbContext) 
        {
            _studentDbContext = studentDbContext;
        }

        public object Name { get; private set; }

        public async Task<int> CreateAsync(Student student)
        {
            _studentDbContext.Students.Add(student);
            await _studentDbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteAsync(Student student)
        {
            _studentDbContext.Students.Remove(student);
            await _studentDbContext.SaveChangesAsync();
            return true;
        }

      

        public async Task<List<Student>> GetAllAsync()
        {
           return await _studentDbContext.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id, bool useNoTracking = false)
        {
            if (useNoTracking)
            return await _studentDbContext.Students.AsNoTracking().Where(student => student.Id == id).FirstOrDefaultAsync();
            else
                return await _studentDbContext.Students.Where(student => student.Id == id).FirstOrDefaultAsync();
        }

        public Task<Student> GetByIdAsync(object id, bool v)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> GetByNameAsync(string name)
        {
            return await _studentDbContext.Students.Where(student => student.Name.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(Student student)
        {
            _studentDbContext.Update(student);
            await _studentDbContext.SaveChangesAsync();
            return student.Id;

        }
    }
}
