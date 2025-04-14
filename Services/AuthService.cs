using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PersonalFinanceApplication.DTOs.Auth;
using PersonalFinanceApplication.DTOs.Users;
using PersonalFinanceApplication.Exceptions;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Models;
using System.Security.Authentication;

namespace PersonalFinanceApplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _automapper;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper automapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher;
            _automapper = automapper;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

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
            var user = _automapper.Map<User>(userDto);
            if (await UserExistsAsync(user.Email))
            {
                throw new AuthException("Username already exists");
            }

            user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);
            await _userRepository.AddAsync(user);

            var authResponse = _automapper.Map<AuthResponseDto>(user);
            return authResponse;
            

        }

        public async Task<bool> UserExistsAsync(string username)
       {
            //     return await _userRepository.Exists(username);
            return true;
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
