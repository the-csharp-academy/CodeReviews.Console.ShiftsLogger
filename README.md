# ShiftsLogger

A full-stack shifts tracking application built with **ASP.NET Core Web API** and a **C# Console UI**. Workers and their shifts can be created, viewed, updated, and deleted through an interactive menu-driven console interface that communicates with the REST API over HTTP.

---

## Features

- **Worker management** — add, view, update, and delete workers
- **Shift management** — log shifts with start/end times, update shift times or reassign a shift to another worker, and delete shifts
- **Shift duration** — automatically calculated from start and end times
- **Per-worker shift history** — view all shifts belonging to a specific worker
- **Input validation** — date/time input is strictly validated against the `dd/MM/yyyy HH:mm:ss` format; end time must be after start time
- **Spectre.Console tables** — shift and worker data displayed in formatted tables in the console
- **Swagger UI** — explore and test the API directly in the browser during development

---

## Tech Stack

| Layer | Technology |
|---|---|
| API | ASP.NET Core 10 Web API |
| ORM | Entity Framework Core 10 |
| Database | SQL Server |
| Object Mapping | AutoMapper 16 |
| API Docs | Swagger / Swashbuckle |
| Console UI | .NET 10 Console App |
| UI Rendering | Spectre.Console |
| HTTP Client | `System.Net.Http.HttpClient` |

---

## Project Structure

```
ShiftsLogger/
├── ShiftsLogger/           # ASP.NET Core Web API
│   ├── Controllers/
│   │   ├── ShiftController.cs      # CRUD endpoints for shifts
│   │   └── WorkerController.cs     # CRUD endpoints for workers
│   ├── DTOs/
│   │   ├── ShiftCreateDTO.cs
│   │   ├── ShiftReadDTO.cs
│   │   ├── WorkerDTO.cs
│   │   └── WorkerReadDTO.cs
│   ├── Models/
│   │   ├── Shift.cs
│   │   └── Worker.cs
│   ├── Profiles/
│   │   ├── ShiftProfile.cs         # AutoMapper profile for Shift
│   │   └── WorkerProfile.cs        # AutoMapper profile for Worker
│   ├── Services/
│   │   ├── IShiftService.cs
│   │   ├── ShiftService.cs
│   │   ├── IWorkerService.cs
│   │   └── WorkerService.cs
│   ├── Data/                       # EF Core DbContext & migrations
│   └── Program.cs
│
└── ShiftsLogger.UI/        # Console client application
    ├── Services/
    │   ├── ShiftApiService.cs      # HTTP calls to shift endpoints
    │   └── WorkerApiService.cs     # HTTP calls to worker endpoints
    ├── DTOs/                       # Client-side DTO mirrors
    ├── ShiftInput.cs               # Shift menu & input handling
    ├── WorkerInput.cs              # Worker menu & input handling
    ├── UserInput.cs                # Main menu
    ├── TableVisualizer.cs          # Spectre.Console table rendering
    ├── InputHelper.cs
    ├── Validator.cs
    └── Program.cs
```

---

## API Endpoints

### Workers — `api/workers`

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/workers` | Get all workers |
| `GET` | `/api/workers/{id}` | Get a worker by ID |
| `GET` | `/api/workers/{workerId}/shifts` | Get all shifts for a worker |
| `POST` | `/api/workers` | Create a new worker |
| `PUT` | `/api/workers/{id}` | Update a worker |
| `DELETE` | `/api/workers/{id}` | Delete a worker |

### Shifts — `api/shifts`

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/shifts` | Get all shifts |
| `GET` | `/api/shifts/{id}` | Get a shift by ID |
| `POST` | `/api/shifts` | Create a new shift |
| `PUT` | `/api/shifts/{id}` | Update a shift |
| `DELETE` | `/api/shifts/{id}` | Delete a shift |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local instance or a connection string to a remote server)

### 1. Configure the Database

Open `ShiftsLogger/appsettings.json` and update the connection string to point to your SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ShiftsLoggerDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 2. Apply Migrations

From inside the `ShiftsLogger/ShiftsLogger/` directory, run:

```bash
dotnet ef database update
```

This will create the `ShiftsLoggerDb` database with the `Workers` and `Shifts` tables.

### 3. Run the API

```bash
cd ShiftsLogger/ShiftsLogger
dotnet run
```

The API will start on `http://localhost:5062` by default.  
Swagger UI is available at `http://localhost:5062/swagger` when running in Development mode.

### 4. Run the Console UI

Open a second terminal and run:

```bash
cd ShiftsLogger/ShiftsLogger.UI
dotnet run
```

> **Note:** The UI expects the API to be running on `http://localhost:5062`. If your API uses a different port, update the `urlBase` variable in `ShiftsLogger.UI/Program.cs`.

---

## Usage

Once the console app starts you'll be presented with the **Main Menu**:

```
MAIN MENU
0 - Exit
1 - Manage workers.
2 - Manage shifts.
```

**Worker menu options:** View all workers · Search by ID · Add worker · Update worker · Delete worker · View a worker's shifts

**Shift menu options:** View all shifts · Search by ID · Add shift · Update shift time · Reassign shift to another worker · Delete shift

When adding or updating a shift, enter dates in **`dd/MM/yyyy HH:mm:ss`** format, for example:

```
11/08/2026 08:00:00
```

Enter `0` at any prompt to go back to the previous menu.

---
