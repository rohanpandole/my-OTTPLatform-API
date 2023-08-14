using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("controller")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("employees")]
        public IEnumerable<string> Get()
        {
            return new List<string> { "rohan", "Kishor", "Pandole"};
        }

        [Authorize]
        [HttpPost("postemployees")]
        public IEnumerable<string> PostMy()
        {
            return new List<string> { "rohan", "Kishor", "Pandole" };
        }
    }
}
