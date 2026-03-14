# ShiftsLogger
This is a simple console application that can be used to create and manage workers' shifts. The application is written in C# and uses an SQLite database with Entity Framework Core for data access.

# How it works
The application consists mainly of a front and backend. The frontend or ShiftsUI project is responsible for displaying UI, capturing and validating user inputs and calling the relevant API endpoints. The backend or ShiftsAPI project is responsible for exposing the API endpoints nessecary for the management of shifts. 

When starting the app you will be greeted by a main menu, with options to create, view, edit and delete shifts, as well as the option to exit the app. 
- Choosing to create a shift will prompt you to enter the starting and ending dates in an international format. Entering an invalid date or an impossible time span (a shift that starts after it ends) will prompt you to try again.
- Choosing to view shifts will present you with a list of all existing shifts in the database.
- Choosing to edit a shift will ask you to select a shift to edit from a prompt, and will then have you edit said shift the same way you would create one.
- Choosing to delete a shift will similarly ask you to pick which shift you would like to delete from a prompt, and subsequently delete it.

# Development thoughts
This project helped me see the value of several new concepts, as well as reinforce the value of concepts I've learned/implemented before
- Swagger/postman useful for API testing before I started creating the frontend allowing me to finalize the backend and iron out issues before switching focus.
- Exception handling useful to prevent application crashing in the event of errors connceting to or interfacing with the API endpoints.
- Working with seperate projects encouraged me to create an intermediary class library for classes that can be used by both projects.
- Seperate projects highlight importance of access modifiers/scope control within classes
