using ShiftLogger.UI.Model;
using Spectre.Console;
using System.Reflection;

namespace ShiftLogger.UI.Menus;

internal class TableGenerator
{
    internal static void ShowTable<T>(List<T> data, string title)
    {
        Console.Clear();

        var table = new Table();

        table.Border(TableBorder.Rounded);
        table.Title($"[#A1E8CC]{title}[/]");

        PropertyInfo[] allProperties = typeof(T).GetProperties();

        var validProperties = allProperties
            .Where(property =>
                data.Any(item =>
                {
                    var value = property.GetValue(item);

                    return value != null && !string.IsNullOrWhiteSpace(value.ToString());
                }))
            .ToList();

        foreach (var property in validProperties)
        {
            string columnName = property.Name;

            table.AddColumn($"[#D1B24D]{columnName}[/]");
        }

        foreach (var item in data)
        {
            var values = validProperties
                .Select(p => p.GetValue(item)?.ToString() ?? "")
                .ToArray();

            table.AddRow(values);
        }

        table.BorderColor(Color.FromHex("#2A628F"));
        AnsiConsole.Write(table);
    }

    internal static Table CreateDetailsTable<T>(T item, string? title) where T : notnull
    {
        var table = new Table();

        if (title != null)
        {
            table.Title($"[#A1E8CC]{title}[/]");
        }

        table.Border(TableBorder.Rounded);
        table.AddColumn("[#D1B24D]Detail[/]");
        table.AddColumn("[#D1B24D]Value[/]");

        PropertyInfo[] properties = typeof(T)
            .GetProperties()
            .Where(p => p.GetIndexParameters().Length == 0)
            .ToArray();

        foreach (var property in properties)
        {
            object? rawValue = property.GetValue(item);

            string value = rawValue switch
            {
                decimal decimalValue => decimalValue.ToString("F1"),
                null => "N/A",
                _ => rawValue.ToString() ?? "N/A"
            };

            table.AddRow(property.Name, value);
        }

        table.BorderColor(Color.FromHex("#2A628F"));

        return table;
    }

    internal static Grid CreateUpdateView(Shift shift)
    {
        var grid = new Grid();

        grid.AddColumn(new GridColumn().Width(70));
        grid.AddColumn(new GridColumn().Width(40));

        var detailsPanel = new Panel(
            TableGenerator.CreateDetailsTable(shift, null))
        {
            Header = new PanelHeader("Current Shift")
        };

        var instructionsPanel = new Panel(
            new Markup(
    @"[yellow]Update Worker Shift[/]

Select the field you want to update.

• Worker Name
• Date
• Start Time
• End Time
• Shift Time

[blue]Save Changes[/] to apply the modifications.
[red]Cancel[/] to discard them."))
        {
            Header = new PanelHeader("Instructions")
        };

        grid.AddRow(detailsPanel, instructionsPanel);

        return grid;
    }
}