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
using Microsoft.AspNetCore.Authorization;
using PersonalFinanceApplication.Repositories;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

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
        private readonly JwtSettings _jwtSettings;
        public UsersController(IUserService userService, IAuthService authService, ILogger<AuthService> logger, IPasswordHasher passwordHasher, IMapper automapper, IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _authService = authService;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _automapper = automapper;
            _jwtSettings = jwtSettings.Value;
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
                return BadRequest(new { Message = "Invalid request format" });

            try
            {
                var user = await _authService.AuthenticateAsync(loginDto.Email, loginDto.Password);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                var token = _authService.GenerateJwtToken(user);
                user.LastLogin = DateTime.UtcNow;
                var id = user.Id;
                Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Only send over HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                    Domain = GetCookieDomain() // Custom method for env-specific domains
                });

                return Ok(new
                {
                    Token = token,
                    User = new { user.Id, user.Username, user.Email },

                    ExpiresIn = _jwtSettings.ExpirationMinutes * 60
                });
            }
            catch (NotFoundException)
            {
                await Task.Delay(Random.Shared.Next(200, 500)); // Prevent timing attacks
                return Unauthorized(new { Message = "Invalid email or password" });
            }
            catch (InvalidCredentialsException)
            {
                await Task.Delay(Random.Shared.Next(200, 500));
                return Unauthorized(new { Message = "Invalid email or password" });
            }
        }

        private string? GetCookieDomain()
        {
            return Request.Host.Host.Contains("localhost")
                ? null
                : ".yourdomain.com"; // Leading dot for subdomains
        }
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Domain = GetCookieDomain()
            });

            return Ok(new { Message = "Successfully logged out" });
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

        
    

