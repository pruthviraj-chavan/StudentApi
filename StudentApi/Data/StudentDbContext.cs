using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;  // for List

using Microsoft.Extensions.Options;
using StudentApi.Models;
using StudentApi.Data.Config;
using StudentApi.Controllers;

namespace StudentApi.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext>options) : base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new StudentConfig()); //table 1

            //modelBuilder.ApplyConfiguration(new NewConfig()); //table 2

            //modelBuilder.ApplyConfiguration(new new1Config()); //table 3



        }

        internal void SavedChangesAsync()
        {
            throw new NotImplementedException();
        }
    }

   
}
