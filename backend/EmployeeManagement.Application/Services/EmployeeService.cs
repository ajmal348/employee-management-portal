using AutoMapper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Enums;
using EmployeeManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace EmployeeManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        public EmployeeService(IRepository<Employee> employeeRepository, IMapper mapper, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            try
            {
                var employees = await _employeeRepository.GetAllAsync(e => e.UserAccount);
                return _mapper.Map<List<EmployeeDto>>(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all employees.");
                throw;
            }
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id, e => e.UserAccount);
                return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting employee by ID: {id}");
                throw;
            }
        }

        public async Task<Guid> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            try
            {
                if (!Enum.TryParse<Role>(dto.Role, true, out var role))
                    throw new ArgumentException("Invalid role.");

                var existingEmployees = await _employeeRepository.GetAllAsync(e => e.UserAccount);
                if (!string.IsNullOrWhiteSpace(dto.Username) &&
                    existingEmployees.Any(e => e.UserAccount?.Username == dto.Username))
                {
                    throw new Exception("Username already exists.");
                }

                var employee = _mapper.Map<Employee>(dto);

                if (!string.IsNullOrWhiteSpace(dto.Username) && !string.IsNullOrWhiteSpace(dto.Password))
                {
                    employee.UserAccount = new User
                    {
                        Username = dto.Username,
                        PasswordHash = Hash(dto.Password),
                        Role = role,
                        Employee = employee
                    };
                }

                await _employeeRepository.AddAsync(employee);
                return employee.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee.");
                throw;
            }
        }

        public async Task UpdateEmployeeAsync(Guid id, CreateEmployeeDto dto)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null)
                    throw new Exception("Employee not found.");

                if (!Enum.TryParse<Department>(dto.Department, true, out _))
                    throw new ArgumentException("Invalid department.");
                var hasUsername = !string.IsNullOrWhiteSpace(dto.Username);

                if (hasUsername)
                {
                    var allEmployees = await _employeeRepository.GetAllAsync(e => e.UserAccount);
                    bool usernameTaken = allEmployees.Any(e =>
                        e.UserAccount != null &&
                        e.UserAccount.Username == dto.Username &&
                        e.Id != employee.Id);

                    if (usernameTaken)
                        throw new Exception("Username is already taken by another employee.");
                }

                _mapper.Map(dto, employee);

                if (hasUsername)
                {
                    if (employee.UserAccount == null)
                    {
                        employee.UserAccount = new User
                        {
                            Username = dto.Username,
                            Role = Role.Employee,
                            Employee = employee
                        };
                    }
                    else
                    {
                        employee.UserAccount.Username = dto.Username;
                        if (Enum.TryParse<Role>(dto.Role, true, out var parsedRole))
                        {
                            employee.UserAccount.Role = parsedRole;
                        }
                        else
                        {
                            throw new ArgumentException("Invalid role provided.");
                        }

                    }
                }

                await _employeeRepository.UpdateAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating employee with ID: {id}");
                throw;
            }
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            try
            {
                await _employeeRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting employee with ID: {id}");
                throw;
            }
        }

        public async Task<EmployeeDto?> GetEmployeeByUsernameAsync(string username)
        {
            try
            {
                var employee = await _employeeRepository.GetSingleOrDefaultAsync(
             e => e.UserAccount != null && e.UserAccount.Username == username,
             e => e.UserAccount
         );
                return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching employee by username: {username}");
                throw;
            }
        }


        #region Helper Methods
        private string Hash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
        #endregion

    }
}
