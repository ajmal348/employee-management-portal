using EmployeeManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Entities
{
    public class Employee
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public Department Department { get; set; }
        public string Designation { get; set; } = null!;
        public DateTime DateOfJoining { get; set; }

        // Navigation property (optional)
        public User? UserAccount { get; set; }
    }
}
