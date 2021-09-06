using APIAngular.DataLayer;
using APIAngular.Dtos;
using APIAngular.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace APIAngular.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly IAuthRepos Repos;

        private readonly IConfiguration configuration;


        public AuthController(IAuthRepos _repos , IConfiguration _configuration)
        {
            Repos = _repos;

            configuration = _configuration;

        }




        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {


            //--Validation Model -------------------------------------------------

            var UserRepos = await Repos.Login(userLoginDto.Username , userLoginDto.Password);

            if (UserRepos == null) return Unauthorized();

            if (!ModelState.IsValid) return BadRequest(ModelState);


			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier , UserRepos.Id.ToString()),

			   new Claim(ClaimTypes.Name, UserRepos.Username)

			};


			var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("SettingsToken:Token").Value));


			SigningCredentials Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512);


			var DescripToken = new SecurityTokenDescriptor
			{

				Subject = new ClaimsIdentity(claims),

				Expires = DateTime.Now.AddDays(1),

				SigningCredentials = Creds,


			};


            var TokenHandler = new JwtSecurityTokenHandler();

			var token = TokenHandler.CreateToken(DescripToken);


			return Ok(new { token = TokenHandler.WriteToken(token) });


		}





        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto) 
        {


            //--Validation Model -------------------------------------------------

            if (!ModelState.IsValid) return BadRequest(ModelState);
           
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await Repos.UserExist(userForRegisterDto.Username))
            {
                return BadRequest("هذا المستخدم مسحل مسبقا !");
            }

            var UserInfo = new User() 
            {
                Username = userForRegisterDto.Username
            };

            var InsertUser = await Repos.Register(UserInfo , userForRegisterDto.Password);

            return StatusCode(201);


        }



    }


}
