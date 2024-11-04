using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentApi.Controllers;
using StudentApi.Models;
using System.Collections.Generic;

namespace StudentApi.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Address).IsRequired(false).HasMaxLength(50);

            builder.HasData(new List<Student>
            {
                new Student { Id = 1, Name = "Venkat", Email = "Example@gmail.com", Address = "Prel" },
                new Student { Id = 2, Name = "raman", Email = "raman@gmail.com", Address = "mumbai" }
            });

            builder.HasOne(x => x.Department).WithMany(x   => x.Students).HasForeignKey(x => x.DepartmentId).HasConstraintName("FK_Students_Department");
        }

    }
}
