## About the app

Employee shift tracking app.
It is a client-server application: the Shifts.Logger console client application and the Shifts.WebAPIs server Web API application.

## Launch

To work with the application, you need to:
Launch Shifts.WebAPIs and ensure that the database connection is correct. This application must run continuously and listen for HTTP requests.
Base adress of server you can find in App.config on stroke "BaseAdress"

After launching the application, launch Shifts.Logger. A console application will open with a menu for selecting CRUD operations. You need to enter the employee’s name, the date and time when their shift starts, and the date and time when their shift ends. The shift duration is calculated automatically.

## Architecture

I have chosen a client-server architecture. The Web API application contains models, a service, and a controller for working with REST API. This application can be connected to any client, be it a console, desktop, or browser application.

 The client application contains the ShiftsAPI class, which communicates with the server application.

## Conclusion

For me, this was one of the most useful tasks at the current moment, as it effectively demonstrates how modern development works. Although simplified, the task conveys the essence of the division of responsibilities between developers of client‑side and backend applications, as well as the role of ASP.NET in this backend.