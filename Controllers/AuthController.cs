using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OTTMyPlatform.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController: Controller
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            List<UserLoginDetail> a = new List<UserLoginDetail>();
            using (var conn = new OttplatformContext())
            {
                a = await conn.UserLoginDetails.ToListAsync();
            }
            return Ok(a);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserLoginDetail myRegister)
        {
            if (myRegister == null)
            {
                return BadRequest();
            }
            using (var con = new OttplatformContext())
            {

                try
                {
                    await con.UserLoginDetails.AddAsync(myRegister);
                    await con.SaveChangesAsync();
                }
                catch(Exception e)
                {
                    throw;
                }
                
            }
            return Ok();
        }

        [HttpPost("VerifyUserGenerateTocken")]
        public async Task<IActionResult> VerifyUserGenerateTocken(UserLoginDetail loginUser)
        {
            UserLoginDetail userLoginDetail = new UserLoginDetail();
            JWTDetail jWTDetail = new JWTDetail()
            {
                UserName=  loginUser.UserName,
                Password =  loginUser.Password
            };
            if(jWTDetail == null)
            {
                jWTDetail.LoginMessage = "Empty credential enterd";
                return (IActionResult)jWTDetail;
            }
            using (var conn = new OttplatformContext())
            {
                userLoginDetail = await conn.UserLoginDetails.Where(a => 
                    a.UserName == jWTDetail.UserName &&
                    a.Password == jWTDetail.Password
                    ).FirstAsync();

                if (userLoginDetail != null)
                {
                    jWTDetail.AccessToken = GetTocken(jWTDetail);
                    jWTDetail.LoginMessage = "Login Success";
                    return Ok(jWTDetail);
                }
                else
                {
                   jWTDetail.LoginMessage = "No Data Posted";
                    return BadRequest(jWTDetail);
                }
            }
        }
        private string GetTocken(JWTDetail loginUser)
        {

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWTConfig:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserName", loginUser.UserName),
                        new Claim("UserPassword", loginUser.Password)
                    };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTConfig:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                            _configuration["JWTConfig:Issuer"],
                            _configuration["JWTConfig:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
