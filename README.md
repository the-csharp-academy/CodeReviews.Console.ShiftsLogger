# ShiftsLogger

A worker shift tracker built as two applications: an ASP.NET Core Web API backed by
Entity Framework Core (code-first, SQLite) and a console app that consumes it over HTTP.

## Stack

- .NET 10 / ASP.NET Core Web API (`ShiftsLogger.Api`)
- Entity Framework Core 10 + SQLite, schema created by migrations
- Swagger UI (Swashbuckle) for endpoint docs, Postman collection for stored tests
- Console front end (`ShiftsLogger.UI`) — `HttpClient` + `System.Text.Json`
- xunit test suite (`ShiftsLogger.Tests`)

## Requirements traceability

| Requirement | Where |
|---|---|
| Two applications (API + UI that calls it) | `ShiftsLogger.Api`, `ShiftsLogger.UI` (no project reference between them — HTTP only) |
| All validation and user input in the UI app | `ShiftsLogger.UI/Validation/ShiftInputValidator.cs` + prompt loops in `Menu/ShiftConsoleApp.cs` |
| Lean controller, logic in a service | `Controllers/ShiftsController.cs` (routing + status codes) → `Services/ShiftService.cs` |
| Code-first database via EF migrations | `Data/ShiftsDbContext.cs` + `Migrations/` |
| try-catch around API calls in the front end | `Services/ShiftApiClient.cs` (`SendAsync` / `ThrowIfErrorAsync`) → `ShiftApiException` |
| Duration calculated from start and end, end validated | `Models/Shift.cs` (`Duration`), `Formatting/ShiftFormatter.cs`, `ValidateTimes` |

## Getting started

```bash
# 1. Terminal A — API (schema is created by migrations; run once first)
cd ShiftsLogger.Api
dotnet ef database update
dotnet run                       # http://localhost:5273, Swagger at /swagger

# 2. Terminal B — console UI
cd ShiftsLogger.UI
dotnet run
```

Override the API address if the port differs:

```bash
SHIFTSLOGGER_API_URL=http://localhost:6000 dotnet run
```

## Tests

```bash
dotnet test
```

## Manual testing

- Swagger UI: `http://localhost:5273/swagger`
- Postman: import `docs/ShiftsLogger.postman_collection.json`, run the collection with the API up
  (7 requests, 11 assertions). Headless alternative: `npx newman run docs/ShiftsLogger.postman_collection.json`

## API

| Method | Route | Success | Failure |
|---|---|---|---|
| GET | `/api/shifts` | 200 + array ordered by start time | — |
| GET | `/api/shifts/{id}` | 200 | 404 |
| POST | `/api/shifts` | 201 + `Location` | 400 |
| PUT | `/api/shifts/{id}` | 200 | 400, 404 |
| DELETE | `/api/shifts/{id}` | 204 | 404 |

Dates are naive local wall-clock values in `yyyy-MM-dd HH:mm` format; a shift's duration is
computed from start and end (never stored). Multi-day shifts are allowed.
