using Microsoft.AspNetCore.Mvc;
using OTTMyPlatform.Models;
using OTTMyPlatform.Models.Responce;
using OTTMyPlatform.Repository.Interface;
namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController: Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        public AuthController(IConfiguration configuration, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _authRepository = authRepository;
        }

        [HttpGet("GetAllUser")]
        public async Task<ActionResult<UserLoginResponce>> GetAllUser()
        {
            UserLoginResponce responce = new UserLoginResponce();
            responce.UserDetail = await _authRepository.GetAllUser();
            responce.StatusCode = 200;
            responce.StatusMessage = "Got All User Details";
            return Ok(responce);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserLoginResponce>> Register(UserLoginDetail myRegister)
        {
            UserLoginResponce responce = new UserLoginResponce();
            if (myRegister == null)
            {
                responce.StatusCode = 500;
                responce.StatusMessage = "Please Enter New Details Again";
                return BadRequest(responce);
            }

            responce = await _authRepository.Register(myRegister);

            if (responce.StatusCode == 200)
            {
                return Ok(responce);
            }
            else{
                return BadRequest(responce);
            }
        }

        [HttpPost("VerifyUserGenerateToken")]
        public async Task<ActionResult<UserLoginResponce>> VerifyUserGenerateTocken(UserLoginDetail loginUser)
        {
            UserLoginResponce responce = new UserLoginResponce();
            {
                responce.UserJWTDetail.UserName = loginUser.UserName;
                responce.UserJWTDetail.Password = loginUser.Password;
            };
            if(responce.UserJWTDetail == null)
            {
                responce.StatusMessage = "Empty credential enterd";
                responce.StatusCode = 500;
                return BadRequest(responce);
            }

            responce = await _authRepository.VerifyUserGenerateTocken(responce, loginUser);

            if(responce.StatusCode == 200)
            {
                return Ok(responce);
            }
            else
            {
                return BadRequest(responce);
            }
        }
    }
}
