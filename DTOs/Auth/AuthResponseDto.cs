using PersonalFinanceApplication.DTOs.Users;

namespace PersonalFinanceApplication.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public UserDTO User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
