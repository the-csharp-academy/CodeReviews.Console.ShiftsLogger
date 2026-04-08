
## Information on how to run the app:

Data: SqlLite is used, so no database is required to be installed. The database file will be created automatically when the app is run for the first time.

App: When the app is ran, you will be introduced to a menu, with several options. Please choose the option you want by using the arrow keys. 

User input: there are instructions on how user input needs to be placed. It is imperative these are followed, otherwise you will reach an error. 
If you get an error you believe is false, please report it to me. 

To run this project: You can run the project by opening the solution in Visual Studio and pressing F5, or pressing the run button, or by using the command line to navigate to the project directory and running `dotnet run`.

Below I have added the readme from my personal repository of this project, which goes into more detail about the project, optimizations, and lessons learned.

# Shifts Logger
This is an app designed to log "shifts" to a database. I can use this for my own personal "shift" logging, which in this case is myself working on SWE projects. 
This app is a full stack design with a minimal API, and a console frontend. This is configured to run on localhost:7064.

**Tech used:** Entity Framework, ASP.NET, Spectre Console, Postman.

Entity Framework used to assist with API creation. Postman was used for API debugging and testing. .Net HttpClient and Spectre Console was used on the frontend. 
This was my first experience with .NET HttpClient and I much enjoyed it over using RESTSharp. Though they felt quite similar, I felt as if this worked smoother, and made more sense in my head.

## Optimizations

As usual, I felt my UI/UX design improved over previous programs Ive developed, and I am constantly learning new ways to make the experience smoother. I would like to improve on the shift input in this app, as I feel date/time entering is tedious. 
I also feel as if that is just a side effect of using the console for this application. 
This is the first app I've developed where I decided to enable copilot. I really enjoyed using copilot, and it really saved me a ton of time when writing boilerplate code. I kept it disabled until now to force myself to learn syntax and not "vibe code".


## Lessons Learned

As this was my first "full stack" application with a separate frontend and backend, that came with a heap of challenges learning how to get the two to interact. I spent alot of time learning how to do that, without directly calling one another. 
This turned out to be well simpler than I anticipated. I really enjoyed the learning process here, as I have been very eager to get into more complex development. I am very very eager to start on a new framework like Blazor or React. 
I do have previous experience with Angular, though very very limited. 