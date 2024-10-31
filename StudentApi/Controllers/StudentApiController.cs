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


using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.Xml;
using AutoMapper;
namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentApiController : ControllerBase
    {
        private readonly ILogger<StudentApiController> logger;
        private readonly StudentDbContext _studentDbContext;
        private readonly IMapper _mapper;
        private readonly IMapper mapper;

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }

        public StudentApiController(ILogger<StudentApiController> logger, StudentDbContext studentDbContext, IMapper mapper)
        {
            this.logger = logger;
            _studentDbContext = studentDbContext;
            _mapper = mapper;
        }


        [HttpGet("All", Name = "Get All Students")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            // Retrieve all students from the static CollegeRepository
            this.logger.LogInformation("GetStudents Method Started");
            //var students = _studentDbContext.Students.ToList();
            var students = await _studentDbContext.Students.ToListAsync();
            //{
            //    Id = s.Id,
            //    Name = s.Name,
            //    Email = s.Email,
            //    Address = s.Address
            //}).ToListAsync(); // Ensure to call ToList() to execute the query

            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);


            return Ok(studentDTOData);
        }

        [HttpGet("{id:int}", Name = "Get Student by Id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); // 400 Bad Request
            }

            var student = await _studentDbContext.Students.FirstOrDefaultAsync(n => n.Id == id);
            if (student == null)
            {
                return NotFound("Id Not Found"); // 404 Not Found
            }

            var studentDTO = _mapper.Map<StudentDTO>(student);
            


            return Ok(studentDTO); // 200 OK with DTO
        }

        [HttpGet("{name:alpha}", Name = "Get Student by Name")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StudentDTO>> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(); // 400 Bad Request
            }

            var student = await _studentDbContext.Students.FirstOrDefaultAsync(n => n.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (student == null)
            {
                return NotFound(); // 404 Not Found
            }

            var studentDTO = _mapper.Map<StudentDTO>(student);

            return Ok(studentDTO); // 200 OK with DTO
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

           
            Student student = _mapper.Map<Student>(dto);

            await _studentDbContext.Students.AddAsync(student);
            await _studentDbContext.SaveChangesAsync();
            dto.Id = student.Id;
            return CreatedAtRoute("Get Student By Id", new {id = dto.Id}, dto);   

        }

        [HttpDelete("{id}", Name = "Delete Student")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); // 400 Bad Request
            }

            var student = await _studentDbContext.Students.FirstOrDefaultAsync(n => n.Id == id);
            if (student == null)
            {
                return NotFound("Id is not present"); // 404 Not Found
            }

            _studentDbContext.Students.Remove(student);
            await _studentDbContext.SaveChangesAsync();
            return Ok(true); // 200 OK with true status
        }



        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<StudentDTO>> UpdateStudent([FromBody] StudentDTO dto)
        {
            
            if (dto == null || dto.Id <= 0)
            {
                return BadRequest();
            }

            var existingstudent = await  _studentDbContext.Students.AsNoTracking().Where(s => s.Id == dto.Id).FirstOrDefaultAsync();
            if (existingstudent == null)
            {
                return NotFound();
            }

            var newRecord = _mapper.Map<Student>(dto);

            _studentDbContext.Students.Update(newRecord);

            //existingstudent.Id = model.Id;
            //existingstudent.Name = model.Name;
            //existingstudent.Email = model.Email;    
            //existingstudent.Address = model.Address;

            await _studentDbContext.SaveChangesAsync();

            return NoContent();

        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<StudentDTO>> UpdateStudentPartial(int id ,[FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {

            if (patchDocument == null || id <= 0)
            {
                return BadRequest();
            }

            var existingstudent = await  _studentDbContext.Students.AsNoTracking().Where(s => s.Id == id).FirstOrDefaultAsync();
            if (existingstudent == null)
            {
                return NotFound();
            }

            var studentDTO = _mapper.Map<StudentDTO>(existingstudent);

            patchDocument.ApplyTo(studentDTO,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            existingstudent = _mapper.Map<Student>(studentDTO);
            _studentDbContext.Students.Update(existingstudent);

            //existingstudent = _mapper.Map<Student>(studentDTO);
            //existingstudent.Name=studentDTO.Name;
            //existingstudent.Email=studentDTO.Email;
            //existingstudent.Address=studentDTO.Address;

            await _studentDbContext.SaveChangesAsync();

            return NoContent();

        }

        //patch is used for individually adding data 
    }
}