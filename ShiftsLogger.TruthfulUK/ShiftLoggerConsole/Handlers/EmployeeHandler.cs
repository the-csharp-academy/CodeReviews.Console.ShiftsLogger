using ShiftLoggerConsole.Api;
using ShiftLoggerConsole.Models;
using ShiftLoggerConsole.Views.Utilities;
using Spectre.Console;

namespace ShiftLoggerConsole.Handlers;
internal class EmployeeHandler
{
    private ManagementApiClient _client;

    public EmployeeHandler()
    {
        _client = new ManagementApiClient();
    }

    public async Task ViewEmployees()
    {
        Helpers.Header($"All Employees");

        List<EmployeeResponse> employees;

        try
        {
            employees = await _client.GetAsync<List<EmployeeResponse>>(endpoint: "employees")
            ?? new List<EmployeeResponse>();
        }
        catch (Exception ex)
        {
            employees = new();
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] {ex.Message}\n");
            Helpers.Pause();
            return;
        }

        if (employees.Count == 0)
        {
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] There are no employees currently in the database.\n");
            Helpers.Pause();
            return;
        }

        var table = new Table();
        table
            .AddColumn("[black on chartreuse1] ID [/]")
            .AddColumn("[black on chartreuse1] Name [/]")
            .AddColumn("[black on chartreuse1] Title [/]")
            .ShowRowSeparators()
            .Border(TableBorder.Minimal)
            .BorderStyle(new Style(Color.Grey23))
            .Expand();

        foreach (var e in employees)
        {
            table.AddRow(
                $"{e.Id}", 
                $"{e.Name}", 
                $"{e.Title}");
        }

        AnsiConsole.Write(table);

        Helpers.Pause();
    }

    public async Task AddEmployee()
    {
        Helpers.Header($"Creating Employee");

        string name = Helpers.PromptNonEmpty("Enter Employee's Name: ");
        string title = Helpers.PromptNonEmpty($"Enter {name}'s Title: ");

        EmployeeRequest newEmployee = new EmployeeRequest
        {
            Name = name,
            Title = title,
        };

        try
        {
            await _client.PostAsync<EmployeeRequest, EmployeeResponse>("employees", newEmployee);
            AnsiConsole.MarkupLine($"\n[chartreuse1]Success:[/] {newEmployee.Name} has been added.\n");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"Error: {ex.Message}");
            Helpers.Pause();
        }

        Helpers.Pause();

    }

    public async Task UpdateEmployee()
    {
        Helpers.Header($"Updating Employee");

        List<EmployeeResponse> employees;

        try
        {
            employees = await _client.GetAsync<List<EmployeeResponse>>(endpoint: "employees")
            ?? new List<EmployeeResponse>();
        }
        catch (Exception ex)
        {
            employees = new();
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] {ex.Message}\n");
            Helpers.Pause();
            return;
        }

        if (employees.Count == 0)
        {
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] There are no employees currently in the database.\n");
            Helpers.Pause();
            return;
        }

        var employee = AnsiConsole.Prompt(
            new SelectionPrompt<EmployeeResponse>()
            .Title("Please select an [chartreuse1]employee[/]:")
            .HighlightStyle(Color.Chartreuse1)
            .SearchPlaceholderText("[gray](Enter to select | Type to search)[/]")
            .EnableSearch()
            .AddChoices(employees));

        Helpers.Header($"Updating {employee.Name} | (Employee ID: {employee.Id})");

        var updateName =
            Helpers.ConfirmationPrompt("Update Employee Name?", defaultValue: false);

        var updateTitle =
            Helpers.ConfirmationPrompt("Update Employee Title?", defaultValue: false);

        EmployeeRequest updatedEmployee = new EmployeeRequest
        {
            Name = employee.Name,
            Title = employee.Title
        };

        if (updateName) updatedEmployee.Name = 
                Helpers.PromptNonEmpty("Enter Employee's Name: ");

        if (updateTitle) updatedEmployee.Title = 
                Helpers.PromptNonEmpty("Enter Employee's Title: ");

        try
        {
            await _client.PutAsync<EmployeeRequest>($"employees/{employee.Id}", updatedEmployee);
            AnsiConsole.MarkupLine($"[chartreuse1]Success:[/] {updatedEmployee.Name} with ID {employee.Id} has been updated.");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"Error: {ex.Message}");
        }

        Helpers.Pause();
    }

    public async Task DeleteEmployee()
    {
        Helpers.Header($"Deleting Employee");

        List<EmployeeResponse> employees;

        try
        {
            employees = await _client.GetAsync<List<EmployeeResponse>>(endpoint: "employees")
            ?? new List<EmployeeResponse>();
        }
        catch (Exception ex)
        {
            employees = new();
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] {ex.Message}\n");
            Helpers.Pause();
            return;
        }

        if (employees.Count == 0)
        {
            AnsiConsole.MarkupLine($"\n[red]Error - Returning to Menu:[/] There are no employees currently in the database.\n");
            Helpers.Pause();
            return;
        }

        var employee = AnsiConsole.Prompt(
            new SelectionPrompt<EmployeeResponse>()
            .Title("Please select an [chartreuse1]employee[/]:")
            .HighlightStyle(Color.Chartreuse1)
            .SearchPlaceholderText("[gray](Enter to select | Type to search)[/]")
            .EnableSearch()
            .AddChoices(employees));

        try
        {
            await _client.DeleteAsync($"employees/{employee.Id}");
            AnsiConsole.MarkupLine($"\n[chartreuse1]Success:[/] Employee {employee.Name} (Employee ID: {employee.Id}) has been successfully deleted.");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"Error: {ex.Message}");
        }

        Helpers.Pause();
    }
}
