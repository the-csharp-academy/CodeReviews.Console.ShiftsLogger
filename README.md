# Shifts Logger

A full-stack application for managing workers and their work shifts, built with ASP.NET Core Web API and a Console UI using Spectre.Console.

## Overview

Shifts Logger is a comprehensive solution for tracking worker shifts with full CRUD operations for both workers and their associated shifts. The application consists of two main components:
- **ShiftsLogger.API**: RESTful API backend with Entity Framework Core and SQL Server
- **ShiftsLogger.UI**: Interactive console-based user interface

## Features

### Worker Management
- ✅ Create new workers
- ✅ View all workers
- ✅ View individual worker details with shifts
- ✅ Update worker information
- ✅ Delete workers (cascade deletes associated shifts)

### Shift Management
- ✅ Add shifts for workers
- ✅ View all shifts for a specific worker
- ✅ Update shift times
- ✅ Delete shifts
- ✅ Client-side validation for shift times

## Tech Stack

### Backend (API)
- **.NET 10.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 10.0**
- **SQL Server**
- **Swagger/OpenAPI** for API documentation

### Frontend (Console UI)
- **.NET 10.0**
- **Spectre.Console** for interactive terminal UI
- **HttpClient** for API communication

## Project Structure

```
STUDY.Console.ShiftsLogger/
├── ShiftsLogger.API/
│   ├── Controllers/
│   │   ├── WorkerController.cs
│   │   └── ShiftController.cs
│   ├── Services/
│   │   ├── WorkerService.cs
│   │   └── ShiftService.cs
│   ├── Models/
│   │   ├── Worker.cs
│   │   ├── Shift.cs
│   │   └── DTOs/
│   ├── Data/
│   │   └── AppDbContext.cs
│   ├── Migrations/
│   └── Program.cs
├── ShiftsLogger.UI/
│   ├── Controller/
│   │   ├── WorkerController.cs
│   │   └── ShiftsController.cs
│   ├── Services/
│   │   ├── WorkerService.cs
│   │   └── ShiftService.cs
│   ├── DTOs/
│   ├── Configuration.cs
│   ├── UserInterface.cs
│   └── Program.cs
└── README.md
```

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or Full)
- A terminal that supports ANSI colors (for UI)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd STUDY.Console.ShiftsLogger/STUDY.Console.ShiftsLogger
```

### 2. Configure Database Connection

Update the connection string in `ShiftsLogger.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ShiftLogger;user id=YOUR_USER;password=YOUR_PASSWORD;Encrypt=true;TrustServerCertificate=true;"
  }
}
```

### 3. Run Database Migrations

```bash
cd ShiftsLogger.API
dotnet ef database update
```

### 4. Start the API

```bash
cd ShiftsLogger.API
dotnet run
```

The API will start at `http://localhost:5298`

### 5. Configure API URL (if different)

If your API runs on a different port, update `ShiftsLogger.UI/Configuration.cs`:

```csharp
public const string ApiUrl = "http://localhost:YOUR_PORT/api";
```

### 6. Start the Console UI

Open a new terminal:

```bash
cd ShiftsLogger.UI
dotnet run
```

## API Endpoints

### Workers

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Worker` | Get all workers |
| GET | `/api/Worker/{id}` | Get worker by ID |
| GET | `/api/Worker/{id}/Details` | Get worker with all shifts |
| POST | `/api/Worker` | Create new worker |
| PUT | `/api/Worker/{id}` | Update worker |
| DELETE | `/api/Worker/{id}` | Delete worker |

### Shifts

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Shift/{id}` | Get shift by ID |
| POST | `/api/Shift` | Create new shift |
| PUT | `/api/Shift/{id}` | Update shift |
| DELETE | `/api/Shift/{id}` | Delete shift |

## API Documentation

When the API is running in Development mode, access Swagger UI at:
- `http://localhost:5298/swagger`

## Usage Examples

### Create a Worker (API)

```bash
curl -X POST http://localhost:5298/api/Worker \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe"}'
```

### Add a Shift (API)

```bash
curl -X POST http://localhost:5298/api/Shift \
  -H "Content-Type: application/json" \
  -d '{
    "startTime":"2026-02-12T09:00:00Z",
    "endTime":"2026-02-12T17:00:00Z",
    "workerId":1
  }'
```

### Console UI Navigation

1. **Main Menu**: View workers, add new workers, or exit
2. **Worker Management**: Update worker, delete worker, or manage shifts
3. **Shift Management**: Add, update, or delete shifts for selected worker

## Database Schema

### Workers Table
- `Id` (int, PK, Identity)
- `Name` (nvarchar(100), Required)

### Shifts Table
- `Id` (int, PK, Identity)
- `StartTime` (datetimeoffset, Required)
- `EndTime` (datetimeoffset, Required)
- `WorkerId` (int, FK, Nullable)

**Relationship**: One Worker → Many Shifts (Cascade Delete)

## Recent Improvements

- ✅ Fixed HttpClient socket exhaustion using static readonly instances
- ✅ Removed circular dependency between WorkerService and ShiftService
- ✅ Fixed return type mismatch in WorkerController
- ✅ Added model validation attributes
- ✅ Improved query efficiency in GetWorkerWithShiftsAsync
- ✅ Fixed URL consistency in service calls

## Contributing

This is a study project from [The C# Academy](https://thecsharpacademy.com/). Feel free to fork and experiment!
