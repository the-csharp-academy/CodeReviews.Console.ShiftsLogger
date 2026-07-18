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

    internal static Table CreateDetailsTable<T>(T item, string? title)
    {
        var table = new Table();

        if(title != null) 
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
            var rawValue = property.GetValue(item);

            string value;

            if (rawValue is decimal decimalValue)
            {
                value = decimalValue.ToString("F1");
            }
            else
            {
                value = rawValue.ToString();
            }

            table.AddRow(property.Name, value);
        }

        table.BorderColor(Color.FromHex("#2A628F"));

        return table;
    }
}
