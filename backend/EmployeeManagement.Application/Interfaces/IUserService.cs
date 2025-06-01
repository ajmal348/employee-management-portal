using EmployeeManagement.API.Models.Responses;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<AuthResult> AuthenticateUserAsync(UserLoginDto dto);
    }
}
