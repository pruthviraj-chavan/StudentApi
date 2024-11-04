using StudentApi.Controllers;

namespace StudentApi.Data.Repository
{
    public interface IStudentRepository : ICollegeRepository<Student>
    {
        //Task<List<Student>> GetAllAsync();
        //Task<Student> GetByIdAsync(int id, bool useNoTracking = false);
        //Task<Student> GetByNameAsync(string name);

        //Task<int> CreateAsync (Student student);
        //Task<int> UpdateAsync (Student student);
        
        //Task SaveChangesAsync();
        //Task<Student> GetByIdAsync(object id, bool v);
        //Task<bool> DeleteAsync(Student student);  //these methods are already present in the common repository and this methods are student table specific only so we removed 

        Task<List<Student>> GetAllByFeeStatusAsync(int FeeStatus);
    }
}
