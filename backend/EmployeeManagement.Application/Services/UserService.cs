using Microsoft.Extensions.Logging;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.API.Models.Responses;

namespace EmployeeManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<AuthResult> AuthenticateUserAsync(UserLoginDto dto)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(dto.Username);
                if (user == null)
                {
                    _logger.LogWarning("Authentication failed. User '{Username}' not found.", dto.Username);
                    return new AuthResult { Success = false, ErrorMessage = "User not found." };
                }

                var isValid = await _userRepository.ValidateCredentialsAsync(dto.Username, dto.Password);
                if (!isValid)
                {
                    _logger.LogWarning("Authentication failed. Invalid credentials for user '{Username}'.", dto.Username);
                    return new AuthResult { Success = false, ErrorMessage = "Invalid credentials." };
                }

                return new AuthResult { Success = true, User = user };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during authentication for user '{Username}'.", dto.Username);
                throw;
            }
        }

    }
}
