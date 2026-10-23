using System.Globalization;
using Spectre.Console;

public sealed class EmployeesView
{
    public void DisplayEmployees(IReadOnlyCollection<ClientEmployee> employees)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[green]Employees[/]");

        if (employees.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No employees found.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("ID")
            .AddColumn("First name")
            .AddColumn("Last name")
            .AddColumn("Role")
            .AddColumn("Status");

        foreach (var employee in employees.OrderBy(employee => employee.Id))
        {
            table.AddRow(
                employee.Id.ToString(CultureInfo.InvariantCulture),
                Markup.Escape(employee.FirstName),
                Markup.Escape(employee.LastName),
                Markup.Escape(employee.Role),
                employee.IsActive ? "Active" : "Inactive");
        }

        AnsiConsole.Write(table);
    }

    public ManagementMenuOption DisplayManagementMenu()
        => AnsiConsole.Prompt(
            new SelectionPrompt<ManagementMenuOption>()
                .Title("[green]Choose an employee action[/]")
                .AddChoices(Enum.GetValues<ManagementMenuOption>())
                .UseConverter(FormatMenuOption));

    public int PromptEmployeeId(string action)
        => AnsiConsole.Prompt(
            new TextPrompt<int>($"Enter the employee ID to {Markup.Escape(action)}:")
                .Validate(id => id > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error("The ID must be greater than zero.")));

    public EmployeeWriteRequest PromptEmployeeDetails()
        => new(
            PromptRequiredText("Enter first name:"),
            PromptRequiredText("Enter last name:"),
            PromptRequiredText("Enter role:"));

    public EmployeeWriteRequest PromptEmployeeUpdate(ClientEmployee employee)
    {
        AnsiConsole.MarkupLine(
            "[grey]Current values are pre-filled. Press Enter to keep a value, or edit it.[/]");

        var firstName = PromptUpdateText("First name:", employee.FirstName);
        var lastName = PromptUpdateText("Last name:", employee.LastName);
        var role = PromptUpdateText("Role:", employee.Role);
        var isActive = AnsiConsole.Confirm(
            "Should this employee be active?",
            employee.IsActive);

        return new EmployeeWriteRequest(firstName, lastName, role, isActive);
    }

    public void DisplayMessage(string message)
        => AnsiConsole.MarkupLine(Markup.Escape(message));

    public void WaitForInput()
    {
        AnsiConsole.MarkupLine("\n[grey]Press any key to continue.[/]");
        AnsiConsole.Console.Input.ReadKey(true);
    }

    private static string PromptRequiredText(string prompt)
        => AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
                .Validate(value => !string.IsNullOrWhiteSpace(value)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("A value is required.")));

    private static string PromptUpdateText(string prompt, string currentValue)
    {
        var value = AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
                .DefaultValue(currentValue)
                .EditableDefaultValue(true)
                .AllowEmpty());

        return string.IsNullOrWhiteSpace(value) ? currentValue : value.Trim();
    }

    private static string FormatMenuOption(ManagementMenuOption option)
        => option switch
        {
            ManagementMenuOption.Add => "Add an employee",
            ManagementMenuOption.Update => "Update an existing employee",
            ManagementMenuOption.Delete => "Delete an employee",
            ManagementMenuOption.Back => "Back",
            _ => option.ToString()
        };
}
