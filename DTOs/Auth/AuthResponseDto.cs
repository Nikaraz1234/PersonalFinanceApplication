using PersonalFinanceApplication.DTOs.Users;

namespace PersonalFinanceApplication.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AccessToken { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}
