# Shift Logger

[The Shift Logger Project](https://thecsharpacademy.com/project/16/phonebook) is a CSharpAcademy roadmap project that requires you to build a ASP.NET Core Web API and a Console Client that utilizes the API.

Developed with ASP.NET Core, Entity Framework Core (API) and Spectre Console (Console Client).

# Requirements:
- [x] This is an application where you should record a worker's shifts.
- [x] You need to create two applications: the Web API and the UI that will call it.
- [x] All validation and user input should happen in the UI app.
- [x] Your API's controller should be lean. Any logic should be handled in a separate "service".
- [x] You should use SQL Server, not SQLite
- [x] You should use the "code first" approach to create your database, using Entity Framework's migrations tool.
- [x] Your front-end project needs to have try-catch blocks around the API calls so it handles unexpected errors (i.e. the API isn't running or returns a 500 error.)

# Dependencies
### **ShiftLoggerConsole**
- Spectre.Console (0.50.0)

### **ShiftLoggerAPI**
- Microsoft.EntityFrameworkCore.SqlServer (9.0.8)
- Microsoft.EntityFrameworkCore.Tools (9.0.8)
- Microsoft.AspNetCore.OpenApi (9.0.9)
- Swashbucke.AspNetCore.Swagger (9.0.4)
- Swashbucke.AspNetCore.SwaggerGen (9.0.4)
- Swashbucke.AspNetCore.SwaggerUI (9.0.4)

# Getting Started

### Prerequisites
- .NET 9.0
- SQL Server (2022 Express Recommended)
- EntityFrameworkCore CLI Tools (dotnet ef)
```bash
dotnet tool install -global dotnet-ef
```
### Clone the repository
```bash
git clone https://github.com/TruthfulUK/CodeReviews.Console.ShiftsLogger.git
```
### Restore Dependencies
```bash
# Console
cd CodeReviews.Console.ShiftsLogger\ShiftsLogger.TruthfulUK\ShiftLoggerConsole
dotnet restore
```
```bash
# API
cd ..\ShiftLoggerAPI\
dotnet restore
```
### Database Setup (\ShiftLoggerAPI)

Configure your desired database connection in `appsettings.Development.json` and then create the database using:
```bash
dotnet ef database update
```
###  Run the API
```bash
dotnet run -lp https
```
Verify the API is running with Swagger:
```bash 
HTTPS: https://localhost:7020/Swagger/
HTTP: http://localhost:5101/Swagger/
```
### Run the Console
Open a new Console window in `\ShiftLoggerConsole` and type `dotnet run` 

Or open the project solution (ShiftLoggerConsole.sln) and run in Visual Studio Code / Visual Studio.

# API Endpoints

### Shifts (`/api/shifts`)
- `GET /api/shifts` - Gets all Shifts
- `GET /api/shifts/{id}` - Get Shift by ID
- `GET /api/employees/{id}/shifts` - Get all Shifts by Employee ID
- `POST /api/shifts` - Create a New Shift
- `PUT /api/shifts/{id}` - Update a Shift
- `DELETE /api/shifts/{id}` - Delete a Shift

### Employees (`/api/employees`)
- `GET /api/employees` - Gets all Employees
- `GET /api/employees/{id}` - Get Employee by ID
- `POST /api/employees` - Create a New Employee
- `PUT /api/employees/{id}` - Update an Employee
- `DELETE /api/employees/{id}` - Delete an Employee

# Resources Used
- [Microsoft Learn - Create a controller-based web API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio)
- [TheCSharpAcademy - Introduction to ASP.NET Core Web APIs](https://www.thecsharpacademy.com/course/3/article/1/500050/False)
- [Entity Framework Tutorial - Entity Framework Core](https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx)
- [Microsoft Learn - Overview of Entity Framework Core)](https://learn.microsoft.com/en-us/ef/core/) 
- [Spectre Console Docs](https://spectreconsole.net/)
- ChatGPT (models: 5, 4o) / Gemini (model: 2.5 Pro) for researching, debugging and best practice questions.