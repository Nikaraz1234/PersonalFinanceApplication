using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceApplication.DTOs.Auth;
using PersonalFinanceApplication.DTOs.Users;
using PersonalFinanceApplication.Exceptions;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace PersonalFinanceApplication.Services
{
    public class AuthService : IAuthService
    {

        private readonly ILogger<AuthService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _automapper;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper automapper, ILogger<AuthService> logger, ITokenService tokenService, JwtSettings jwtSettings)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher;
            _automapper = automapper;
            _logger = logger;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings;
        }
        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email.Trim().ToLower());

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var passwordValid = _passwordHasher.VerifyPassword(user.PasswordHash, password);
            if (!passwordValid)
            {
                throw new InvalidCredentialsException("Invalid password");
            }

            return user;
        }

        public async Task<AuthResponseDto> RegisterAsync(UserRegisterDTO userDto)
        {
            try
            {
                _logger.LogInformation("Starting registration for {Email}", userDto.Email);
                var normalizedEmail = userDto.Email.Trim().ToLower();
                var normalizedUsername = userDto.Username.Trim();

                // Check if email exists
                _logger.LogInformation("Checking if email exists...");
                if (await _userRepository.EmailExists(normalizedEmail))
                {
                    throw new AuthException("Email already registered");
                }

                // Check if username exists
                _logger.LogInformation("Checking if username exists...");
                if (await _userRepository.Exists(normalizedUsername))
                {
                    throw new AuthException("Username already exists");
                }

                _logger.LogInformation("Hashing password...");
                var hashedPassword = _passwordHasher.HashPassword(userDto.Password);

                _logger.LogInformation("Creating user object...");
                var user = new User
                {
                    Email = normalizedEmail,
                    Username = normalizedUsername,
                    PasswordHash = hashedPassword,
                    CreatedAt = DateTime.UtcNow,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Budgets = new List<Budget>(),
                    Transactions = new List<Transaction>(),
                    SavingsPots = new List<SavingsPot>(),
                    RecurringBills = new List<RecurringBill>()
                };

                _logger.LogInformation("Saving user to database...");
                await _userRepository.AddAsync(user);
                _logger.LogInformation("User registered successfully with ID: {UserId}", user.Id);

                _logger.LogInformation("Generating JWT token...");
                var (token, expiry) = _tokenService.CreateToken(user);

                _logger.LogInformation("Returning auth response...");
                return new AuthResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    CreatedAt = user.CreatedAt,
                    AccessToken = token,
                    TokenExpiry = expiry
                };
            }
            catch (AuthException ex)
            {
                _logger.LogWarning("Registration failed - {Message}", ex.Message);
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error during registration: {Message}", dbEx.Message);

                if (dbEx.InnerException != null)
                {
                    _logger.LogError("Inner exception: {Message}", dbEx.InnerException.Message);
                }

                throw new AuthException("Database error occurred. Please check unique constraints or data formats.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception during registration for {Email}", userDto.Email);

                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception: {Message}", ex.InnerException.Message);
                }

                throw new AuthException($"Registration failed. Please try again. [DEBUG: {ex.Message}]");
            }
        }

        public async Task<bool> UserExistsAsync(string username)
       {
            return await _userRepository.Exists(username);
       }

        public async Task ChangePasswordAsync(string username, string oldPassword, string newPassword)
        {
            var user = await AuthenticateAsync(username, oldPassword);
            user.PasswordHash = _passwordHasher.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
        }
        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
 

            var user = await _userRepository.GetByEmailAsync(dto.Email);


            if (user == null || !ValidateResetToken(user, dto.Token))
            {
                await Task.Delay(new Random().Next(200, 500));
                return;
            }


            user.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetExpires = null;

            await _userRepository.UpdateAsync(user);

        }


        private bool ValidateResetToken(User user, string token)
        {
            return user.PasswordResetToken == token
                   && user.PasswordResetExpires > DateTime.UtcNow;
        }


    }
}
