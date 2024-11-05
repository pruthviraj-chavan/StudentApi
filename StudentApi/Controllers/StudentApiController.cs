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
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Net;
namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors(PolicyName = "AllowOnlyGoogle")] //for origin we use cors policy
    //[EnableCors()] //default policy
    [Authorize(AuthenticationSchemes = "LoginForLocal", Roles = "Superadmin , Admin")] //who can access this 
    public class StudentApiController : ControllerBase
    {
        private readonly ILogger<StudentApiController> logger;
        //private readonly IStudentRepository _studentRepository;
        //private readonly ICollegeRepository<Student> _studentRepository;
        private readonly IStudentRepository _studentRepository;
        private ApiResponse _apiResponse;

        private readonly IMapper _mapper;

        //private ICollegeRepository<Student> studentRepository;

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }

        public StudentApiController(ILogger<StudentApiController> logger, IMapper mapper, IStudentRepository studentRepository)
        {
            this.logger = logger;
            _studentRepository = studentRepository;

            _mapper = mapper;
            _apiResponse = new();
        }


        [HttpGet("All", Name = "Get All Students")]
        //[AllowAnonymous] //any one can accesss this method without authentication

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            // Retrieve all students from the static CollegeRepository
            this.logger.LogInformation("GetStudents Method Started");
            //var students = _studentDbContext.Students.ToList();
            var students = await _studentRepository.GetAllAsync();


            _apiResponse.Data = _mapper.Map<List<StudentDTO>>(students);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;


            return Ok(_apiResponse);
        }

        [HttpGet("{id:int}", Name = "Get Student by Id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        //[EnableCors()] //for this method only
        //[DisableCors()] //for disabling cors for particular method
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest(); // 400 Bad Request
            }

            var student = await _studentRepository.GetAsync(student => student.Id == id);
            if (student == null)
            {
                return NotFound("Id Not Found"); // 404 Not Found
            }

            _apiResponse.Data = _mapper.Map<StudentDTO>(student);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;



            return Ok(_apiResponse); // 200 OK with DTO
        }

        [HttpGet("{name:alpha}", Name = "Get Student by Name")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        //[EnableCors()] //for this method only
        //[DisableCors()] //for disabling cors for particular method
        public async Task<ActionResult<StudentDTO>> GetStudentByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(); // 400 Bad Request
            }

            var student = await _studentRepository.GetAsync(student => student.Name.ToLower().Contains(name));
            if (student == null)
            {
                return NotFound(); // 404 Not Found
            }

            _apiResponse.Data = _mapper.Map<StudentDTO>(student);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return Ok(_apiResponse); // 200 OK with DTO
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        //[EnableCors()] //for this method only
        //[DisableCors()] //for disabling cors for particular method
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }


            Student student = _mapper.Map<Student>(dto);

            var studentAfterCreation = await _studentRepository.CreateAsync(student);

            dto.Id = studentAfterCreation.Id;
            _apiResponse.Data = dto;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return CreatedAtRoute("Get Student By Id", new { id = dto.Id }, _apiResponse);

        }

        [HttpDelete("{id}", Name = "Delete Student")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        //[EnableCors()] //for this method only
        //[DisableCors()] //for disabling cors for particular method
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            try
            {
                //BadRequest - 400 - Badrequest - Client error
                if (id <= 0)
                    return BadRequest();

                var student = await _studentRepository.GetAsync(student => student.Id == id);
                //NotFound - 404 - NotFound - Client error
                if (student == null)
                    return NotFound($"The student with id {id} not found");

                await _studentRepository.DeleteAsync(student);
                _apiResponse.Data = true;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                //OK - 200 - Success
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return Ok(_apiResponse);
            }

        }



        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        //[EnableCors()] //for this method only
        //[DisableCors()] //for disabling cors for particular method
        public async Task<ActionResult<StudentDTO>> UpdateStudent([FromBody] StudentDTO dto)
        {

            try
            {
                if (dto == null || dto.Id <= 0)
                    BadRequest();

                var existingStudent = await _studentRepository.GetAsync(student => student.Id == dto.Id, true);

                if (existingStudent == null)
                    return NotFound();

                var newRecord = _mapper.Map<Student>(dto);

                await _studentRepository.UpdateAsync(newRecord);

                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return Ok(_apiResponse);
            }

        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        //[EnableCors()] //for this method only
        //[DisableCors()] //for disabling cors for particular method
        public async Task<ActionResult<StudentDTO>> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {

            try
            {
                if (patchDocument == null || id <= 0)
                    BadRequest();

                var existingStudent = await _studentRepository.GetAsync(student => student.Id == id, true);

                if (existingStudent == null)
                    return NotFound();

                var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

                patchDocument.ApplyTo(studentDTO, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                existingStudent = _mapper.Map<Student>(studentDTO);

                await _studentRepository.UpdateAsync(existingStudent);

                //204 - NoContent
                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return Ok(_apiResponse);
            }

        }
    }
}