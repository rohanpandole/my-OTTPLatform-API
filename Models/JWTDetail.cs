namespace WebApplication1.Models
{
    public class JWTDetail
    {
        public int? UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int? PhoneNumber { get; set; }
        public string? LoginMessage { get; set; }
        public string? AccessToken { get; set; }
    }
}