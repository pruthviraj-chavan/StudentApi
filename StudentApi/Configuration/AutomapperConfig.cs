using System.Runtime;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using StudentApi.Controllers;
using StudentApi.Models;
namespace StudentApi.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig() 
        {
            //CreateMap<Student,StudentDTO>().ReverseMap();
            //CreateMap<StudentDTO,Student>();
            //CreateMap<StudentDTO,Student>().ReverseMap().ForMember(n => n.Name, opt => opt.Ignore()); //ignoring property name 
            //CreateMap<StudentDTO, Student>().ReverseMap()AddTransform<string>(n => string.IsNullOrEmpty(n) ? "no address found" : n);
            //CreateMap<StudentDTO, Student>().ReverseMap();

            CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.Address, opt => opt.MapFrom(n => string.IsNullOrEmpty(n.Address) ? "no address found" : n.Address)); //used for particular record to show no address found
        }
    }
}
