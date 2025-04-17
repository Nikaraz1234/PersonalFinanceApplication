using System;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApplication.DTOs.Users;
using PersonalFinanceApplication.DTOs.Auth;
using PersonalFinanceApplication.Interfaces;
using PersonalFinanceApplication.Exceptions;
using PersonalFinanceApplication.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using AutoMapper;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthService> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _automapper;
        public UsersController(IUserService userService, IAuthService authService, ILogger<AuthService> logger, IPasswordHasher passwordHasher, IMapper automapper)
        {
            _userService = userService;
            _authService = authService;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _automapper = automapper;
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserSummaryDTO>))]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }



        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }



        [HttpGet("{id}/profile")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfileDTO))]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            var profile = await _userService.GetUserProfileAsync(id);
            return profile == null ? NotFound() : Ok(profile);
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDto)
        {
            _logger.LogInformation("Received registration request for {Email}", registerDto.Email);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for registration: {@ModelState}", ModelState);
                return BadRequest("Invalid input data.");
            }
            try
            {
                var authResponse = await _authService.RegisterAsync(registerDto);
                _logger.LogInformation("Registration completed successfully for {Email}", registerDto.Email);
                return CreatedAtAction(nameof(GetUser), new { id = authResponse.Id }, authResponse);
            }
            catch (AuthException ex)
            {
                _logger.LogWarning("Registration rejected: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for {Email}", registerDto.Email);

                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner exception details: {InnerExceptionMessage}", ex.InnerException.Message);
                    _logger.LogError("Inner exception stack trace: {InnerStackTrace}", ex.InnerException.StackTrace);
                }

                return StatusCode(500, $"Registration failed: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User user;
            try
            {
                user = await _authService.AuthenticateAsync(loginDto.Email, loginDto.Password);
            }
            catch (NotFoundException)
            {
                return BadRequest("Invalid email or password.");
            }
            catch (InvalidCredentialsException)
            {
                return BadRequest("Invalid email or password.");
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.Username),
    };

            var token = _authService.GenerateJwtToken(user);

            // Return token in response
            return Ok(new
            {
                Token = token,
                User = new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                }
            });
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return Ok("Logged out");
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest("ID mismatch");

            try
            {
                await _userService.UpdateUserAsync(id, updateDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Initiate password reset
        /// </summary>

        /*
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok(); // Always return OK for security
        }
        */
        /// Complete password reset
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _authService.ResetPasswordAsync(dto);
                return Ok(new { Message = "Password reset successfully" });
            }
            catch (Exception ex)
            {

                return BadRequest(new { Message = "Error processing request" });
            }
        }
    }
}

        
    

