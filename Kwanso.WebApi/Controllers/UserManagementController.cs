using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Kwanso.Model.Poco;
using Kwanso.Model.ViewModel;
using Kwanso.Repository;
using Kwanso.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Kwanso.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagement : ControllerBase
    {
        private IConfiguration _config;
        private IUserRepository userRepository;

        public UserManagement(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            this.userRepository = userRepository;
        }


        [HttpGet("/user/{Id}")]
        [ProducesResponseType(200, Type = typeof(Users))]
        public IActionResult Get(int Id)
        {
            var user = userRepository.Get(Id);

            return Ok(user != null? new Users {Id = user.Id,Email = user.Email }:new Users());
        }

        [AllowAnonymous]
        [HttpPost("/register")]
        [ProducesResponseType(200, Type = typeof(Users))]
        [Produces("application/json")]
        public IActionResult register([FromBody] UserVm login)
        {
            Users user = new Users
            {
                Email = login.Email,
                Password = login.Password,
                Created_At = DateTime.Now,
                Updated_At = DateTime.Now,
            };

            var users = userRepository.Create(user);
            user.Password = null;
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public IActionResult login([FromBody] UserVm login)
        {
            Users user = null;
            if (login != null)
            {
                user = userRepository.GetAll().Where(c => c.Email == login.Email && c.Password == login.Password).FirstOrDefault();
            }

            string tokens = "";
            if (user != null)
            {
                tokens = JWTToken(user.Email);
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(tokens);
        }

        private string JWTToken(string user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Issuer",_config["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.UniqueName,user)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}