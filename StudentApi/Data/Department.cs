using StudentApi.Controllers;

namespace StudentApi.Data
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Student> Students { get; set; } //used as foriegn key reference
    
    }
}
