using PersonalFinanceApplication.DTOs.Auth;
using PersonalFinanceApplication.DTOs.Users;
using PersonalFinanceApplication.Models;
using PersonalFinanceApplication.Repositories;

namespace PersonalFinanceApplication.Interfaces
{
    public interface IUserService
    {
        /* User Management */
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<UserProfileDTO> GetUserProfileAsync(int userId);
        Task<IEnumerable<UserSummaryDTO>> GetAllUsersAsync();
        Task<UserSummaryDTO> GetUserSummaryAsync(int userId);
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<UserDTO> GetUserByEmailAsync(string email);
        /* Registration & Profile */
        Task<AuthResponseDto> RegisterAsync(UserRegisterDTO registerDto);
        Task<UserDTO> UpdateUserAsync(int id, UserUpdateDTO updateDto);
        Task DeleteUserAsync(int id);

        /* Password Management */
        Task RequestPasswordResetAsync(ForgotPasswordDto forgotPasswordDto);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        /* User Data */
        Task<decimal> GetUserTotalBalanceAsync(int userId);
        Task<decimal> GetUserMonthlyIncomeAsync(int userId);
        Task<decimal> GetUserMonthlyExpensesAsync(int userId);
    }
}
