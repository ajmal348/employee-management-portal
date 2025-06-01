using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeProfileController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeProfileController> _logger;

        public EmployeeProfileController(IEmployeeService employeeService, ILogger<EmployeeProfileController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the profile of the currently authenticated employee.
        /// </summary>
        /// <returns>The employee's profile.</returns>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOwnProfile()
        {
            try
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrWhiteSpace(username))
                {
                    _logger.LogWarning("Unauthorized access attempt to employee profile.");
                    return Unauthorized(new { message = "You must be logged in to access this resource." });
                }

                var employee = await _employeeService.GetEmployeeByUsernameAsync(username);
                if (employee == null)
                {
                    _logger.LogWarning("Employee profile not found for username: {Username}", username);
                    return NotFound(new { message = "Profile not found." });
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile for current user.");
                return StatusCode(500, new { message = "An error occurred while retrieving your profile." });
            }
        }      

    }
}
