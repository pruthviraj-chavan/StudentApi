//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using StudentApi.Models;
//using System.ComponentModel.DataAnnotations;

//namespace StudentApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StudentApiController : ControllerBase
//    {
//        [HttpGet("All", Name = "Get All Students")]

//        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
//        {


//            var students = CollegeRepository.Students.Select(s => new StudentDTO()
//            {
//                Id = s.Id,
//                Name = s.Name,
//                Email = s.Email,
//                Address = s.Address
//            });
//            return Ok(students);
//        }


//        [HttpGet("{id:int}", Name = "Get All Students by Id")]
//        [ProducesResponseType(200)]
//        [ProducesResponseType(400)]
//        [ProducesResponseType(404)]
//        public ActionResult<StudentDTO> GetStudentById(int id)
//        {
//            if (id <= 0)
//            {
//                return BadRequest(); //400 code
//            }

//            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
//            if (student == null)
//            {
//                return NotFound("Id Not Found");
//            }
//            var studentDTO = new StudentDTO()
//            {
//                Id = student.Id,
//                Name = student.Name,
//                Email = student.Email,
//                Address = student.Address
//            };

//            return Ok(student); //200 success code
//        }



//        [HttpGet("{Name:alpha}", Name = "GetByName")]
//        [ProducesResponseType(200)]
//        [ProducesResponseType(400)]
//        [ProducesResponseType(404)]
//        public ActionResult<StudentDTO> GetStudentByName(string Name )
//        {
//            if (string.IsNullOrEmpty(Name))
//            {
//                return BadRequest();
//            }
//            var student = CollegeRepository.Students.FirstOrDefault(n => n.Name == Name);

//            if (student == null)
//            {
//                return NotFound(); //404 not found
//            }

//            var studentDTO = new StudentDTO()
//            {
//                Id = student.Id,
//                Name = student.Name,
//                Email = student.Email,
//                Address = student.Address
//            };
//            return Ok(student);
//        }






//        [HttpDelete("{id}", Name = "DeleteStudent")]
//        [ProducesResponseType(200)]
//        [ProducesResponseType(400)]
//        [ProducesResponseType(404)]
//        [ProducesResponseType(500)]

//        public ActionResult<bool> DeleteStudent(int id)
//        {
//            if (id <= 0)
//            {
//                return BadRequest();
//            }

//            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
//            if (student == null)
//            {
//                return NotFound("id is not present");
//            }

//            CollegeRepository.Students.Remove(student);

//            return Ok(true);
//        }


//    }
//}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Data;
using StudentApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentApiController : ControllerBase
    {
        private readonly ILogger<StudentApiController> logger;
        private readonly StudentDbContext _studentDbContext;

        public StudentApiController(ILogger<StudentApiController> logger, StudentDbContext studentDbContext)
        {
            this.logger = logger;
            _studentDbContext = studentDbContext;
        }


        [HttpGet("All", Name = "Get All Students")]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            // Retrieve all students from the static CollegeRepository
            this.logger.LogInformation("GetStudents Method Started");
            //var students = _studentDbContext.Students.ToList();
            var students = _studentDbContext.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Address = s.Address
            }).ToList(); // Ensure to call ToList() to execute the query

            return Ok(students);
        }

        [HttpGet("{id:int}", Name = "Get Student by Id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); // 400 Bad Request
            }

            var student = _studentDbContext.Students.FirstOrDefault(n => n.Id == id);
            if (student == null)
            {
                return NotFound("Id Not Found"); // 404 Not Found
            }

            var studentDTO = new StudentDTO()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address
            };

            return Ok(studentDTO); // 200 OK with DTO
        }

        [HttpGet("{name:alpha}", Name = "Get Student by Name")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<StudentDTO> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(); // 400 Bad Request
            }

            var student = _studentDbContext.Students.FirstOrDefault(n => n.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (student == null)
            {
                return NotFound(); // 404 Not Found
            }

            var studentDTO = new StudentDTO()
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Address = student.Address
            };

            return Ok(studentDTO); // 200 OK with DTO
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

           
            Student student = new Student()
            {
                
                Name = model.Name,
                Email = model.Email,
                Address = model.Address
            };

            _studentDbContext.Students.Add(student);
            _studentDbContext.SaveChanges();    
            model.Id = student.Id;
            return CreatedAtRoute("Get Student By Id", new {id = model.Id},model);   

        }

        [HttpDelete("{id}", Name = "Delete Student")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<bool> DeleteStudent(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); // 400 Bad Request
            }

            var student = _studentDbContext.Students.FirstOrDefault(n => n.Id == id);
            if (student == null)
            {
                return NotFound("Id is not present"); // 404 Not Found
            }

            _studentDbContext.Students.Remove(student);
            _studentDbContext.SaveChanges();
            return Ok(true); // 200 OK with true status
        }



        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<StudentDTO> UpdateStudent([FromBody] StudentDTO model)
        {
            
            if (model == null || model.Id <= 0)
            {
                return BadRequest();
            }

            var existingstudent= _studentDbContext.Students.Where(s => s.Id == model.Id).FirstOrDefault();
            if (existingstudent == null)
            {
                return NotFound();
            }

            existingstudent.Id = model.Id;
            existingstudent.Name = model.Name;
            existingstudent.Email = model.Email;    
            existingstudent.Address = model.Address;

            _studentDbContext.SaveChanges();

            return NoContent();

        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<StudentDTO> UpdateStudentPartial(int id ,[FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {

            if (patchDocument == null || id <= 0)
            {
                return BadRequest();
            }

            var existingstudent = _studentDbContext.Students.Where(s => s.Id == id).FirstOrDefault();
            if (existingstudent == null)
            {
                return NotFound();
            }

            var studentDTO = new StudentDTO()
            {
                Id = existingstudent.Id,
                Name = existingstudent.Name,
                Email = existingstudent.Email,
                Address = existingstudent.Address
            };

            patchDocument.ApplyTo(studentDTO,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            

            existingstudent.Name=studentDTO.Name;
            existingstudent.Email=studentDTO.Email;
            existingstudent.Address=studentDTO.Address;
            _studentDbContext.SaveChanges();

            return NoContent();

        }


    }
}