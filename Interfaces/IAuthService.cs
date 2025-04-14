using PersonalFinanceApplication.DTOs.Auth;
using PersonalFinanceApplication.DTOs.Users;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<AuthResponseDto> RegisterAsync(UserRegisterDTO user);
        Task<bool> UserExistsAsync(string username);
        Task ChangePasswordAsync(string username, string oldPassword, string newPassword);
        Task ResetPasswordAsync(ResetPasswordDto dto);

    }
}
