# Employee Management Portal

This is a full-stack demo project built as part of a **technical assessment**. It demonstrates clean architecture, proper API design, and a responsive user interface.

> **Tech Stack:**  
> 🔹 Frontend: Angular 20  
> 🔹 Backend: ASP.NET Core Web API (C#)  
> 🔹 Database: EF Core + SQL Server  
> 🔹 Auth: JWT Token-based authentication

---

## 🎯 Objective

To implement a role-based employee portal using .NET and Angular, with the following core functionalities:

### Admin
- Login
- Add, edit, delete employees
- Assign login credentials (username & password)

### Employee
- Login
- View their own profile

---

## 🚀 Features
- ✅ Authentication using JWT
- ✅ Role-based access control (Admin / Employee)
- ✅ Responsive UI for desktop and mobile
- ✅ Form validations with Angular reactive forms
- ✅ SweetAlert2 notifications
- ✅ Clean folder structure (backend & frontend separated)

## 🧪 Default Credentials

| Role     | Username  | Password    |
| -------- | --------- | ----------- |
| Admin    | admin     | admin123    |
| Employee | employee1 | password123 |

---

## 🛠️ Getting Started

### 🔧 Backend Setup (`ASP.NET Core`)

1. Open a terminal and navigate to the backend folder:
   ```bash
   cd backend
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

4. Run the API:
   ```bash
   dotnet run
   ```

   The backend will run on: `https://localhost:7053`

---

### 🌐 Frontend Setup (`Angular 20`)

1. Open a terminal and navigate to the frontend folder:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   ng serve
   ```

   The app will be available at: `http://localhost:4200`

---

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js (v18+)](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli) installed globally:
  ```bash
  npm install -g @angular/cli
  ```
- SQL Server (LocalDB or full version)