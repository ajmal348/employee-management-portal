using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <returns>List of employees.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        /// <summary>
        /// Retrieves a specific employee by ID.
        /// </summary>
        /// <param name="id">The ID of the employee.</param>
        /// <returns>The employee details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                _logger.LogWarning("Employee with ID {Id} not found.", id);
                return NotFound(new { message = "Employee not found", id });
            }

            return Ok(employee);
        }

        /// <summary>
        /// Creates a new employee with optional credentials.
        /// </summary>
        /// <param name="dto">The employee creation request.</param>
        /// <returns>201 Created with new employee ID.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            try
            {
                var id = await _employeeService.CreateEmployeeAsync(dto);
                _logger.LogInformation("Employee created with ID {Id}", id);

                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create employee.");
                return StatusCode(500, new { message = "An error occurred while creating the employee." });
            }
        }

        /// <summary>
        /// Updates an existing employee by ID.
        /// </summary>
        /// <param name="id">The ID of the employee.</param>
        /// <param name="dto">The updated employee data.</param>
        /// <returns>Updated employee details.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateEmployeeDto dto)
        {
            try
            {
                await _employeeService.UpdateEmployeeAsync(id, dto);
                var updated = await _employeeService.GetEmployeeByIdAsync(id);
                if (updated == null)
                {
                    return NotFound(new { message = "Employee not found after update", id });
                }

                _logger.LogInformation("Employee with ID {Id} updated.", id);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update employee with ID {Id}", id);
                return StatusCode(500, new { message = "An error occurred while updating the employee." });
            }
        }

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        /// <param name="id">The ID of the employee.</param>
        /// <returns>Success or failure status.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var existing = await _employeeService.GetEmployeeByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Attempted to delete non-existent employee with ID {Id}", id);
                    return NotFound(new { message = "Employee not found", id });
                }

                await _employeeService.DeleteEmployeeAsync(id);
                _logger.LogInformation("Employee with ID {Id} deleted.", id);
                return Ok(new { message = "Employee deleted successfully", id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete employee with ID {Id}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the employee." });
            }
        }
        [HttpGet("exists")]
        [AllowAnonymous] // Optional: Allow checking username without login
        public async Task<IActionResult> CheckUsernameExists([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { message = "Username is required" });

            var existing = await _employeeService.GetEmployeeByUsernameAsync(username);
            return Ok(new { exists = existing != null });
        }
    }
}
