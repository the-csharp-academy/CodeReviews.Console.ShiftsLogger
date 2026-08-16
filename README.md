# ShiftsLogger

> Console client and ASP.NET Core Web API for managing workers' shifts.

# Requeriments

- [x] This is an application where you should record a worker's shifts.
- [x] You need to create two applications: the Web API and the UI that will call it.
- [x] All validation and user input should happen in the UI app.
- [x] Your API's controller should be lean. Any logic should be handled in a separate "service".
- [x] You should use the "code first" approach to create your database, using Entity Framework's migrations tool.
- [x] Your front-end project needs to have try-catch blocks around the API calls so it handles unexpected errors (i.e. the API isn't running or returns a 500 error.)

# API ShiftLoggers

The ShiftsLogger API is a RESTful ASP.NET Core Web API designed to manage employee work shifts.
It provides endpoints to create, retrieve, update, and delete shift records while enforcing business rules through input validation and error handling.

The API uses Entity Framework Core with SQLite for data persistence, 
follows a layered architecture (Controllers, Services, DTOs, and Models),
and exposes interactive documentation through Swagger. It also includes logging with `ILogger`, proper HTTP status codes,
and automatic management of timestamps such as `CreatedAt` and `UpdateAt`.

## API Endpoints

| Method | Endpoint | Description | Success Response |
|:------:|----------|-------------|:----------------:|
| **GET** | `/api/workershifts/ViewAll` | Returns all worker shifts. | `200 OK` |
| **GET** | `/api/workershifts/ViewOne={id}` | Returns a worker shift by its ID. | `200 OK` |
| **POST** | `/api/workershifts/Create` | Creates a new worker shift. | `201 Created` |
| **PATCH** | `/api/workershifts/Update{id}` | Updates one or more fields of an existing worker shift. | `200 OK` |
| **DELETE** | `/api/workershifts/Delete{id}` | Deletes a worker shift by its ID. | `200 OK` |

## Validation

-   Worker name cannot be empty.
-   Worker name cannot contain digits or special characters.
-   Start and end time cannot be equal.
-   Date and time formats are validated.
-   Worked hours are calculated automatically.

## Error Handling

-   Validation errors (400)
-   Resource not found (404)
-   Internal server errors (500)
-   Logging with `ILogger`
-   Client-side `try/catch` around API calls.

# Console UI ShiftLoggers

The ShiftsLogger Console Client is a .NET application built with Spectre.Console that provides an intuitive and interactive interfacefor managing worker shifts.
It communicates with the REST API through `HttpClient`, allowing users to perform all CRUD operations without interacting directly with the database.

The client includes input validation, exception handling, formatted tables and panels, interactive menus,
and clear feedback messages to ensure a user-friendly experience.

## Menu Navigation

The application is organized around a simple menu system built with **Spectre.Console**.

The main menu allows users to:

- View all worker shifts
- View a single worker shift
- Create a new worker shift
- Update an existing worker shift
- Delete a worker shift
- Exit the application

Each option guides the user through the required steps using interactive prompts, validation messages, and formatted tables to ensure a clear and
intuitive experience.

### Main Menu

<p align="center">
  <img width="302" height="223" alt="imagen" src="https://github.com/user-attachments/assets/6545381e-1358-4722-8d75-3933b7639e7b" />
<</p>

### List Worker Shifts

<p align="center">
  <img width="1105" height="840" alt="imagen" src="https://github.com/user-attachments/assets/1c3abb06-a7ab-4cfb-85c9-d7e289744435" />
</p>

### View One Worker Shift

<p aling="center">
  <img width="590" height="375" alt="imagen" src="https://github.com/user-attachments/assets/0f34687f-1a85-4ee6-8dca-f173799664a7" />
</p>

### Create Worker Shift

<p align="center">
  <img width="557" height="492" alt="imagen" src="https://github.com/user-attachments/assets/b4e889a0-0121-41bb-a346-36824e124774" />
</p>

### Update Worker Shift

<p align="center">
  <img width="721" height="418" alt="imagen" src="https://github.com/user-attachments/assets/670c5a78-a897-40c4-95c5-2524447ec64b" />
  <img width="1596" height="668" alt="imagen" src="https://github.com/user-attachments/assets/857a7380-ab59-44a0-a092-5d248ef17462" />
</p>

### Delete Worker Shift

<p align="center">
  <img width="717" height="467" alt="imagen" src="https://github.com/user-attachments/assets/6687c0fb-f130-45c1-8c8a-e3582a89947f" />
  <img width="640" height="495" alt="imagen" src="https://github.com/user-attachments/assets/1187d047-ad3f-41d8-b8e5-66758bf5637a" />
</p>

# Challenges

- Handling HTTP errors that may occur when querying the API.
- Build an interactive menu that is user-friendly when using the console UI.
- Use a logger to keep a record of the operations performed on the API.
- Use PACTH to update a record.

# What I have learned

- Design and consume RESTful APIs using ASP.NET Core and `HttpClient`.
- Handle exceptions gracefully on both the API and client applications.
- Use `ILogger` to log application events and unexpected errors.
- Perform partial updates using the `PATCH` HTTP method.

# Areas to improve

- CI/CD with GitHub Actions.
- Enhanced logging and monitoring.
- Integration tests.
- Docker support.

# Resourced Used

- Microsoft Learn - ASP.NET Core Web API documentation: https://learn.microsoft.com/aspnet/core/web-api/
- Microsoft Learn - Entity Framework Core documentation: https://learn.microsoft.com/ef/core/
- Microsoft Learn - HttpClient documentation: https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient
- Microsoft Learn - System.Text.Json documentation: https://learn.microsoft.com/dotnet/standard/serialization/system-text-json-overview
- Microsoft Learn - Logging in .NET with ILogger: https://learn.microsoft.com/dotnet/core/extensions/logging
- Microsoft Learn - DateOnly and TimeOnly: https://learn.microsoft.com/dotnet/standard/datetime/how-to-use-dateonly-timeonly
- Microsoft Learn - REST API Best Practices: https://learn.microsoft.com/azure/architecture/best-practices/api-design
- Spectre.Console Documentation: https://spectreconsole.net/
- Swagger / OpenAPI Documentation: https://swagger.io/docs/
- SQLite Documentation: https://www.sqlite.org/docs.html
- NUnit Documentation: https://docs.nunit.org/
- The C# Academy - ShiftsLogger Project: https://www.thecsharpacademy.com/project/18/shifts-logger
