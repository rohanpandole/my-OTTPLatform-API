using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OTTMyPlatform.Models;
using OTTMyPlatform.Models.Responce;
using OTTMyPlatform.Repository.Interface;
using OTTMyPlatform.Repository.Interface.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

namespace OTTMyPlatform.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDBContext _context;
        public AuthRepository(IConfiguration configuration, IDBContext dBContext)
        {
            _configuration = configuration;
            _context = dBContext;
        }
        public async Task<List<UserLoginDetail>> GetAllUser()
        {
            List<UserLoginDetail> users = new List<UserLoginDetail>();
            using (var conn = new OttplatformContext())
            {
                users = await conn.UserLoginDetails.ToListAsync();
            }
            return users;
        }

        public async Task<UserLoginResponce> Register(UserLoginDetail myRegister)
        {
            UserLoginResponce responce = new UserLoginResponce();
            using (var con = new OttplatformContext())
            {
                try
                {
                    await con.UserLoginDetails.AddAsync(myRegister);
                    await con.SaveChangesAsync();
                    responce.StatusCode = 200;
                    responce.StatusMessage = "New User Created";
                }
                catch (Exception e)
                {
                    responce.StatusCode = 500;
                    responce.StatusMessage = "Failed To Create New User!";
                    return responce;
                }
            }
            return responce;
        }
        public async Task<UserLoginResponce> VerifyUserGenerateTocken(UserLoginResponce responce, UserLoginDetail loginUser)
        {
            UserLoginDetail userLoginDetail = new UserLoginDetail();
            try
            {
                using (var conn = new OttplatformContext())
                {
                    userLoginDetail = await conn.UserLoginDetails.Where(a =>
                        a.UserName == loginUser.UserName &&
                        a.Password == loginUser.Password
                        ).FirstAsync();

                    responce.UserJWTDetail.AccessToken = GetTocken(responce.UserJWTDetail);
                    responce.StatusMessage = "Login Success";
                    responce.UserJWTDetail.UserID = userLoginDetail.Id;
                    responce.StatusCode = 200;
                    return responce;
                }
            }
            catch (Exception ex)
            {
                responce.StatusMessage = "No User Found or " + ex.Message;
                responce.StatusCode = 500;
                return responce;

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
