using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors(PolicyName = "AllowOnlyMicrosoft")]
    [Authorize(AuthenticationSchemes = "LoginForMicrosoft", Roles = "Superadmin , Admin")]
    public class MicrosoftController : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
      
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public ActionResult Get()
        {
            return Ok("This is microsoft");
        }

    }
}
