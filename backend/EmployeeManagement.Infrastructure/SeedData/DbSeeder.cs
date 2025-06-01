using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Enums;
using EmployeeManagement.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagement.Infrastructure.SeedData
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Users.Any()) return;

            // Admin user
            var adminEmployee = new Employee
            {
                FirstName = "Super",
                LastName = "Admin",
                Email = "admin@example.com",
                Phone = "9876543210",
                Department = Department.IT,
                Designation = "System Administrator",
                DateOfJoining = DateTime.Parse("2020-01-01")
            };

            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = Hash("admin123"),
                Role = Role.Admin,
                Employee = adminEmployee
            };

            // List of other employee records
            var employeeUsers = new List<User>
            {
                CreateEmployee("Arjun", "Mehta", "arjun.mehta@example.com", "9876543210", Department.IT, "Software Engineer", "2020-06-15", "arjun", "password123"),
                CreateEmployee("Priya", "Singh", "priya.singh@example.com", "9876509876", Department.HR, "HR Manager", "2019-03-10", "priya", "password123"),
                CreateEmployee("Rahul", "Verma", "rahul.verma@example.com", "9812345678", Department.Finance, "Accountant", "2021-01-05", "rahul", "password123"),
                CreateEmployee("Sneha", "Das", "sneha.das@example.com", "9834567890", Department.Marketing, "Marketing Executive", "2022-07-20", "sneha", "password123"),
                CreateEmployee("Karan", "Kapoor", "karan.kapoor@example.com", "9798989898", Department.IT, "Backend Developer", "2020-09-01", "karan", "password123"),
                CreateEmployee("Neha", "Reddy", "neha.reddy@example.com", "9823456789", Department.Admin, "Office Admin", "2018-11-11", "neha", "password123"),
                CreateEmployee("Aman", "Jain", "aman.jain@example.com", "9800123456", Department.Sales, "Sales Executive", "2021-04-17", "aman", "password123"),
                CreateEmployee("Divya", "Nair", "divya.nair@example.com", "9845678901", Department.Legal, "Legal Advisor", "2019-08-25", "divya", "password123"),
                CreateEmployee("Rohan", "Roy", "rohan.roy@example.com", "9811122233", Department.IT, "UI/UX Designer", "2022-01-15", "rohan", "password123"),
                CreateEmployee("Anjali", "Sharma", "anjali.sharma@example.com", "9871123456", Department.HR, "Recruiter", "2023-02-12", "anjali", "password123"),
                CreateEmployee("Demo", "Employee", "employee1@example.com", "9000000001", Department.IT, "Test User", "2024-01-01", "employee1", "password123")
            };

            context.Users.Add(adminUser);
            context.Users.AddRange(employeeUsers);
            context.SaveChanges();
        }

        private static User CreateEmployee(string firstName, string lastName, string email, string phone, Department department, string designation, string dateOfJoining, string username, string password)
        {
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                Department = department,
                Designation = designation,
                DateOfJoining = DateTime.Parse(dateOfJoining)
            };

            return new User
            {
                Username = username,
                PasswordHash = Hash(password),
                Role = Role.Employee,
                Employee = employee
            };
        }

        private static string Hash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
