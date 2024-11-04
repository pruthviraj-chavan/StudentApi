using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Data;
using StudentApi.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentApi.Controllers
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public int? DepartmentId { get; set; }

        public virtual Department? Department { get; set; }  
    }

}