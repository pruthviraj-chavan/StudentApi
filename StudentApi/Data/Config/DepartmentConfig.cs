using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentApi.Controllers;

namespace StudentApi.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.DepartmentName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1200);
           

            builder.HasData(new List<Department>
            {
                new Department { Id = 1, DepartmentName = "Venkat",  Description = "Prel" },
                new Department { Id = 2, DepartmentName = "raman",  Description = "mumbai" }
            });
        }
    }
}
