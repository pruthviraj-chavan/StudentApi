﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors(PolicyName = "AllowOnlyGoogle")] //for origin we use cors policy
    //[EnableCors()] //default policy
    //[Authorize(Roles = "Superadmin , Admin")] //who can access this 

    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration; 
        public LoginController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        [HttpPost]
        public ActionResult Login(LoginDTO model)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest("invalid credentials");
            }

            LoginResponseDTO response = new () { Username = model.Username};

            byte[] key = null;
            if (model.Policy == "Local")
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTLocal"));
            else if (model.Policy == "Microsoft")
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTMicrosoft"));
            else if (model.Policy == "Google")
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTGoogle"));



            //var key3 = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTLocal"));
            //var key1 = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTGoogle"));
            //var key2 = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTMicrosoft"));



            if (model.Username == "Infomatics" && model.Password == "Infomatics123")
            {
                

                //var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretKey"));

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.Role,"Admin")
                    }),
                    Expires = DateTime.Now.AddHours(4),
                    SigningCredentials = new(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha512Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.token = tokenHandler.WriteToken(token);
            }

            else
            {
                return Ok("invalid username and password");
            }
            return Ok(response);
        }
    }

   
}