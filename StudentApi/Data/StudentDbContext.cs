using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;  // for List

using Microsoft.Extensions.Options;
using StudentApi.Models;
using StudentApi.Data.Config;
using StudentApi.Controllers;
using StudentApi.Data.Repository;

namespace StudentApi.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext>options) : base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<User> users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RolePrivilege> RolePrivileges { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new StudentConfig()); //table 1

            modelBuilder.ApplyConfiguration(new DepartmentConfig()); //table 2

            modelBuilder.ApplyConfiguration(new UserConfig()); //table 3

            modelBuilder.ApplyConfiguration(new RoleConfig());

            modelBuilder.ApplyConfiguration(new RolePrivilegeConfig());



        }

        
    }

   
}
