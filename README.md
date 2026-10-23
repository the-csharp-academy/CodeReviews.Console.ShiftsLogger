# Shifts Logger

A .NET 9 application for managing employees and their work shifts. It consists of an ASP.NET Core Web API backed by SQLite and a Spectre.Console client that communicates with the API over HTTP.

This project was created as part of the [C# Academy Shifts Logger project](https://www.thecsharpacademy.com/project/17/shifts-logger).

## Features

- Create, view, update, and delete employees and shifts.
- Archive and restore employees by setting their active status.
- Prevent new shifts from being assigned to inactive or nonexistent employees.
- Allow existing shifts to remain assigned to an inactive employee unless the assignment changes.
- Validate that a shift's end time is later than its start time.
- Calculate shift duration from its start and end times.
- Display employee details alongside shifts.
- Provide Swagger documentation for the API in the Development environment.
- Include API controller tests using an in-memory SQLite database.

## Project Structure

- `ShiftsLogger.API` — ASP.NET Core Web API and SQLite data layer.
- `ShiftsLogger.Client` — Console application for managing employees and shifts.
- `ShiftsLogger.API.Tests` — NUnit tests for API controllers and shift behavior.

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Run the Application

Start the API first:

```sh
dotnet run --project ShiftsLogger.API --launch-profile http
```

The API listens at `http://localhost:5048`. In Development, Swagger UI is available at `http://localhost:5048/swagger`.

In a second terminal, start the console client:

```sh
dotnet run --project ShiftsLogger.Client
```

The client API address is configured in `ShiftsLogger.Client/Configuration/appsettings.json` under `ShiftsDB:BaseUrl`. Update it if the API is running at a different address.

## Console Client

The client opens with a portal menu where you can choose **Shifts**, **Employees**, or **Back**. Each section lists its records and offers options to add, update, or delete them.

- Shift listings show the employee's name and role, start and end times, and calculated duration.
- Creating or reassigning a shift checks that the employee exists and is active before asking for the shift times.
- Shift times must use a supported date/time format, and the end must be later than the start.
- Update prompts show existing values so you can keep them or enter replacements. Employee updates also let you change active status.
- Client errors from the API are shown with explanatory messages.

## Database

The API uses SQLite with the connection string in `ShiftsLogger.API/appsettings.json`. On startup, it applies EF Core migrations and seeds example employees and shifts when the corresponding tables are empty. The database file is named `Shifts.db`.

## API Endpoints

The API request examples are in `ShiftsLogger.API/ShiftsLogger.API.http`.

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/shifts` | Get all shifts, including employee details and calculated duration |
| `GET` | `/api/shifts/{id}` | Get a shift by ID |
| `POST` | `/api/shifts` | Create a shift |
| `PUT` | `/api/shifts/{id}` | Update a shift |
| `DELETE` | `/api/shifts/{id}` | Delete a shift |
| `GET` | `/api/employees` | Get all employees |
| `GET` | `/api/employees/{id}` | Get an employee by ID |
| `POST` | `/api/employees` | Create an employee |
| `PUT` | `/api/employees/{id}` | Update an employee |
| `PATCH` | `/api/employees/{id}/archive` | Mark an employee inactive |
| `PATCH` | `/api/employees/{id}/restore` | Mark an employee active |
| `DELETE` | `/api/employees/{id}` | Delete an employee |

## API Tests

The NUnit test project covers the API controllers and their persistence behavior using an isolated in-memory SQLite database. Tests include CRUD outcomes, missing-record responses, employee archive/restore and active-status updates, shift assignment rules, time-range validation, and the calculated shift response data.

Run the API tests with:

```sh
dotnet test ShiftsLogger.API.Tests/ShiftsLogger.API.Tests.csproj
```
