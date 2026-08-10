# Shifts Logger

A small system for logging work shifts, made up of two projects:

- **`silvermax.shiftlogger`** — ASP.NET Core Web API backed by SQL Server (EF Core).
- **`silvermax.shiftlogger.UI`** — a console app (Spectre.Console) that talks to the API over HTTP to add, view, update, and delete shifts.

The two projects are independent processes — the console app is not a class library reference, it calls the API over the network. **The API must be running before you start the UI.**

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server reachable locally (LocalDB, SQL Server Express, or a full instance)
- (Optional, if you don't want to use Visual Studio's Package Manager Console for migrations) the EF Core CLI tool:
  ```
  dotnet tool install --global dotnet-ef
  ```

## 1. Clone and restore

```
git clone <repo-url>
cd CodeReviews.Console.ShiftsLogger
dotnet restore
```

## 2. Configure the database connection

Check `silvermax.shiftlogger/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Shifts;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

If your SQL Server instance isn't a default local instance (e.g. you use SQL Server Express), change `Server=localhost` to match it, for example `Server=localhost\SQLEXPRESS`.

## 3. Apply database migrations

Migrations are **not** applied automatically on startup — you need to run them once before the API will work.

Using the .NET CLI, from the repo root:

```
dotnet ef database update --project silvermax.shiftlogger --startup-project silvermax.shiftlogger
```

Or, in Visual Studio's Package Manager Console:

```
Update-Database -Project silvermax.shiftlogger -StartupProject silvermax.shiftlogger
```

This creates the `Shifts` database and its `Shifts` table.

## 4. Run the API

⚠️ **Start the API using the `https` launch profile.** The console UI's `HttpClient` is hardcoded to `https://localhost:7203/` — running the API on the `http`-only profile means the UI won't be able to reach it.

From Visual Studio: set `silvermax.shiftlogger` as the startup project and run the **https** profile (the dropdown next to the Run button).

From the CLI:

```
cd silvermax.shiftlogger
dotnet run --launch-profile https
```

Once it's up, confirm it at `https://localhost:7203/swagger`.

## 5. Run the console UI

With the API still running, in a separate terminal:

```
cd silvermax.shiftlogger.UI
dotnet run
```

Or set `silvermax.shiftlogger.UI` as the startup project in Visual Studio and run it (F5 / Ctrl+F5).

> To launch both projects together with a single F5 in Visual Studio: right-click the solution → **Properties** → **Startup Project** → **Multiple startup projects** → set both projects' Action to *Start*.

## Troubleshooting

- **UI throws a connection error / can't reach the server** — the API isn't running, or it's running on the `http` profile instead of `https`. See step 4.
- **"Invalid object name 'Shifts'" or similar database errors** — migrations haven't been applied yet. See step 3.
- **The UI's `.dll` is "blocked" when you try to run it** — this can happen with files synced through cloud storage (e.g. OneDrive). Right-click the `.dll` → Properties → check "Unblock", or run:
  ```powershell
  Get-ChildItem -Recurse -File .\silvermax.shiftlogger.UI\bin | Unblock-File
  ```

## Tech stack

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core + SQL Server
- Spectre.Console (console UI)
