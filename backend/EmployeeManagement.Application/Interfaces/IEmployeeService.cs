using EmployeeManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id);
        Task<Guid> CreateEmployeeAsync(CreateEmployeeDto dto);
        Task UpdateEmployeeAsync(Guid id, CreateEmployeeDto dto);
        Task DeleteEmployeeAsync(Guid id);
        Task<EmployeeDto?> GetEmployeeByUsernameAsync(string username);

    }
}
