using WebApplication1.Models;

namespace OTTMyPlatform.Models.Responce
{
    public class UserLoginResponce
    {
        public List<UserLoginDetail> UserDetail { get; set; }
        public JWTDetail UserJWTDetail { get; set; }
        public UserLoginResponce()
        {
            UserDetail = new List<UserLoginDetail>();
            UserJWTDetail = new JWTDetail();
        }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
    }
}
