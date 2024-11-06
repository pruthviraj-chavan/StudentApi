using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Data;
using StudentApi.Data.Repository;
using StudentApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Role> _roleRepository;
        private readonly ApiResponse apiResponse;

        public RoleController(IMapper mapper, ICollegeRepository<Role> roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            apiResponse = new ApiResponse();
        }

        [HttpPost]
        [Route("Created")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> CreateRoleAsync(RoleDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            Role role = _mapper.Map<Role>(dto);
            role.IsDeleted = false;
            role.CreatedDate = DateTime.Now;
            role.ModifiedDate = DateTime.Now;

            var result = await _roleRepository.CreateAsync(role);
            dto.Id = result.Id;

            apiResponse.Data = dto;
            apiResponse.Status = true;
            apiResponse.StatusCode = HttpStatusCode.OK;

            return Ok(apiResponse);
            //return CreatedAtRoute("GetRoleById", new { id = dto.Id }, apiResponse);
        }

        [HttpGet]
        [Route("All",Name ="GetAllRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ApiResponse>> GetRolesAsync()
        {
            
            var roles = await _roleRepository.GetAllAsync();

            apiResponse.Data = roles;
            apiResponse.Status = true;
            apiResponse.StatusCode = HttpStatusCode.OK;

            return Ok(apiResponse);

        }
    }
}
