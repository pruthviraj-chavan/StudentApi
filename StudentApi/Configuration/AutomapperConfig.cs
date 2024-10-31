using System.Runtime;
using AutoMapper;
using StudentApi.Controllers;
using StudentApi.Models;
namespace StudentApi.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig() 
        {
            CreateMap<Student,StudentDTO>().ReverseMap();
            //CreateMap<StudentDTO,Student>();

        }
    }
}
