using AutoMapper;
using PersonalFinanceApplication.DTOs.Auth;
using PersonalFinanceApplication.DTOs.Users;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserProfileDTO> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetUserWithTransactionsAsync(userId);
            var profileDto = _mapper.Map<UserProfileDTO>(user);

            // Calculate financial summaries
            profileDto.TotalBalance = await GetUserTotalBalanceAsync(userId);
            profileDto.MonthlyIncome = await GetUserMonthlyIncomeAsync(userId);
            profileDto.MonthlyExpenses = await GetUserMonthlyExpensesAsync(userId);

            return profileDto;
        }

        public async Task<IEnumerable<UserSummaryDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserSummaryDTO>>(users);
        }
        public async Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return _mapper.Map<UserDTO>(user);
        }
        public async Task<UserSummaryDTO> GetUserSummaryAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return _mapper.Map<UserSummaryDTO>(user);
        }

        public async Task<AuthResponseDto> RegisterAsync(UserRegisterDTO registerDto)
        {
            if (await _userRepository.EmailExists(registerDto.Email))
                throw new Exception("Email already registered");

            var user = _mapper.Map<User>(registerDto);
            await _userRepository.AddAsync(user);

            return new AuthResponseDto
            {
     
            };
        }

        public async Task<UserDTO> UpdateUserAsync(int id, UserUpdateDTO updateDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            _mapper.Map(updateDto, user);
            await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task RequestPasswordResetAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email);
            if (user != null)
            {
                // Generate and store reset token
                // Send email with reset link
            }
        }

        public async Task<decimal> GetUserTotalBalanceAsync(int userId)
        {
            var user = await _userRepository.GetUserWithTransactionsAsync(userId);
            return user?.Transactions.Sum(t => t.Amount) ?? 0;
        }

        public async Task<decimal> GetUserMonthlyIncomeAsync(int userId)
        {
            var user = await _userRepository.GetUserWithTransactionsAsync(userId);
            var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return user?.Transactions
                .Where(t => t.Amount > 0 && t.Date >= firstDayOfMonth)
                .Sum(t => t.Amount) ?? 0;
        }

        public async Task<decimal> GetUserMonthlyExpensesAsync(int userId)
        {
            var user = await _userRepository.GetUserWithTransactionsAsync(userId);
            var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return user?.Transactions
                .Where(t => t.Amount < 0 && t.Date >= firstDayOfMonth)
                .Sum(t => Math.Abs(t.Amount)) ?? 0;
        }
        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userRepository.GetByEmailAsync(resetPasswordDto.Email);
            if (user == null) return; // Silent fail for security

            // In real implementation: Validate reset token first
            await _userRepository.UpdatePasswordHashAsync(user.Id,
                BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword));
            await _userRepository.UpdateLastLoginAsync(user.Id);
        }
    }
}