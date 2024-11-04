using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Data;
using StudentApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.Xml;
using AutoMapper;
using StudentApi.Data.Repository;
namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentApiController : ControllerBase
    {
        private readonly ILogger<StudentApiController> logger;
        private readonly IStudentRepository _studentRepository;
       
        private readonly IMapper _mapper;
        private readonly IMapper mapper;

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }

        public StudentApiController(ILogger<StudentApiController> logger, IMapper mapper , IStudentRepository studentRepository)
        {
            this.logger = logger;
            _studentRepository = studentRepository;
            
            _mapper = mapper;
        }


        [HttpGet("All", Name = "Get All Students")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            // Retrieve all students from the static CollegeRepository
            this.logger.LogInformation("GetStudents Method Started");
            //var students = _studentDbContext.Students.ToList();
            var students = await _studentRepository.GetAllAsync();
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

            var student = await _studentRepository.GetByIdAsync(id);
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

            var student = await _studentRepository.GetByNameAsync(name);
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

            var id = await _studentRepository.CreateAsync(student);
          
            dto.Id = id;
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

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound("Id is not present"); // 404 Not Found
            }

           
            await _studentRepository.DeleteAsync(student);
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

            var existingstudent = await  _studentRepository.GetByIdAsync(dto.Id,true);
            if (existingstudent == null)
            {
                return NotFound();
            }

            var newRecord = _mapper.Map<Student>(dto);

          

            //existingstudent.Id = model.Id;
            //existingstudent.Name = model.Name;
            //existingstudent.Email = model.Email;    
            //existingstudent.Address = model.Address;

            await _studentRepository.UpdateAsync(newRecord);

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

            var existingstudent = await _studentRepository.GetByIdAsync(id, true);
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
         

            //existingstudent = _mapper.Map<Student>(studentDTO);
            //existingstudent.Name=studentDTO.Name;
            //existingstudent.Email=studentDTO.Email;
            //existingstudent.Address=studentDTO.Address;

            await _studentRepository.UpdateAsync(existingstudent);

            return NoContent();

        }

        //patch is used for individually adding data 
    }
}