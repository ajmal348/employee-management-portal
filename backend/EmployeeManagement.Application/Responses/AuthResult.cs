using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.API.Models.Responses
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public User? User { get; set; }
    }

}
