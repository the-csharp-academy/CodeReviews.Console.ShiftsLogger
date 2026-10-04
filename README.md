# ShiftLogger

**C# Web API + Console Application | Entity Framework Core (Code-First) | SQL Server**

An application for recording workers and their shifts, built as part of the C# Academy learning path. It is made of two programs: a Web API that keeps the data in a SQL Server database, and a console app that the user works with. This was the first project where I built my own API and the app that uses it.

---

## How to Run

You need the .NET SDK (the projects use net10.0), SQL Server (Express or LocalDB), and the Entity Framework command line tool:

```
dotnet tool install --global dotnet-ef
```

1. Clone the repository.
2. Open `appsettings.json` in the API project and put your own SQL Server in `ConnectionStrings:DefaultConnection`. The default is:
   ```json
   "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ShiftLogger;Trusted_Connection=True;TrustServerCertificate=True;"
   ```
3. From the API folder (`ShiftLogger`), create the database with the migrations that are in the repository:
   ```
   dotnet ef database update
   ```
4. Still in the API folder, start the API:
   ```
   dotnet run
   ```
   The API runs at `http://localhost:5011` by default (check the "Now listening on" line). You can try it in the browser with Swagger at `http://localhost:5011/swagger`.
5. Open a second terminal, go to the UI folder (`ShiftLoggerUI`) and run:
   ```
   dotnet run
   ```

Both programs have to be running at the same time. If your API uses a different port, change the address `http://localhost:5011` in the two UI files `WorkerService.cs` and `ShiftService.cs`.

The database starts empty, so add a worker first, because every shift belongs to a worker.

---

## What the App Does / How It Works

The console app has a main menu with two sub-menus:

- **Workers:** list all workers, get one worker, see the shifts of one worker, add, edit and delete
- **Shifts:** list all shifts, get one shift, add, edit and delete

Every shift is shown with how long it lasted, for example `8h 30m`. The app works this out from the start and end time each time, so it is never saved in the database and can never be wrong.

The console app never touches the database itself. It sends a request to the API, the API reads or saves the data, and the app shows the answer. If the API is not running, the app shows an error message and keeps going instead of crashing.

Whenever the app asks for an ID, typing `0` cancels and goes back to the menu. After each action the app waits for a key press, so the result stays on screen before the menu comes back.

### Checks on what the user types

All the checks happen in the console app, before anything is sent to the API:

- **Names** can have letters, spaces, hyphens and apostrophes, and must start with a letter. "Jean-Luc" and "O'Brien" work, "Bob$" does not.
- **IDs** must be numbers, and the app asks the API if that worker or shift really exists. If not, it asks again.
- **Dates** must be written as `dd-MM-yyyy - HH:mm:ss`, for example `15-03-2026 - 09:00:00`.
- **The end of a shift** must be after its start. If it is not, the app asks for both times again.

The API does the most important checks again, because tools like Postman do not go through the console app. It refuses a shift that ends before it starts and a shift for a worker that does not exist.

---

## Architectural Choices

- **Two separate programs.** The console app and the API are two projects that only talk to each other over HTTP. That is why the console app has its own small `Worker` and `Shift` classes.
- **Controllers, service and database each have one job.** The controllers only decide what to answer (found, not found, bad request). The service in the API does the real work: the checks and all the reading and saving. Only the service talks to the database, so the controllers stay short.
- **The service reports and the controller answers.** The service cannot send an HTTP answer, so it just gives back a result, for example nothing when something is missing. The controller turns that into a "not found".
- **An empty list is not an error.** "Not found" is only used when one specific worker or shift does not exist. A worker without shifts gets an empty list, and the app says "There are no shifts yet".
- **The API sends shifts without the worker attached.** A shift points to its worker, who points to their shifts, which point back to the worker, and so on forever. Sending all of that crashed the API, so each shift is sent on its own.
- **Code-First database.** The tables are created from the `Worker` and `Shift` classes with migrations, so I did not write any SQL by hand. One worker has many shifts.
- **Menus built with enums.** The menu options are enums used with Spectre.Console, so a typo in an option is caught before the program even runs.

---

## Changes After Review

After the first review I made these changes:

1. **Added this README.**
2. **Moved the checks into the console app.** The end of a shift must be after its start, IDs are checked against the API before anything is sent, and names accept letters, spaces, hyphens and apostrophes. The user gets a clear message straight away instead of an error from the API.
3. **Took the database out of the controllers.** All the reading and saving now lives in the service, and the controllers only choose the answer.

I also added the shift duration and the option to cancel any ID question by typing 0.

---

## What I Learned

This was my first Web API, and a lot of it was about understanding where each job belongs:

- Two programs that only talk over HTTP is different from one big program. Both have to be running, and the address in the console app has to match the one the API uses.
- My first version had the controllers saving to the database themselves. Moving that into the service made me see why the controller should only decide the answer, and why the service cannot return "not found" on its own.
- When the console app only says "500 Internal Server Error", the real reason is in the terminal where the API is running. That is where I found the endless loop between workers and shifts.
- A "not found" and an empty list are not the same thing. Sending "not found" for a worker without shifts made the console app show an error, when it should just say there are no shifts.
- Checking that an input has the right format is not the same as checking that it makes sense. A date can be written correctly and still end before it starts.
- A validation loop that does not ask for new input inside the loop never ends. I got that wrong with the start and end times before I fixed it.
- Testing the API with Swagger and Postman first made it much easier to tell if a bug was in the API or in the console app. Also, the API does not pick up code changes while it runs, so it has to be restarted every time.
