using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="dto">User login credentials (username and password).</param>
        /// <returns>JWT token on successful login.</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var result = await _userService.AuthenticateUserAsync(dto);

                if (!result.Success)
                {
                    _logger.LogWarning("Login failed for username: {Username} - Reason: {Reason}", dto.Username, result.ErrorMessage);
                    return Unauthorized(new { message = result.ErrorMessage });
                }

                var token = _jwtService.GenerateToken(result.User!);
                _logger.LogInformation("User '{Username}' logged in successfully.", result.User!.Username);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for username: {Username}", dto.Username);
                return StatusCode(500, new { message = "An error occurred during login." });
            }
        }
    }
}
