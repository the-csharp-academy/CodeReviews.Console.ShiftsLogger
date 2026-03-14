using ShiftsUI.Services;
using ShiftsUI.Views;
using static ShiftsUI.Enums.MainMenuEnums;
using ClassLibray.Models;

namespace ShiftsUI.Controllers;

internal class UIController
{
    private readonly IShiftView _shiftView;
    private readonly IShiftUIService _shiftUIService;
    public UIController(IShiftView ShiftView, IShiftUIService ShiftUIService)
    {
        _shiftView = ShiftView;
        _shiftUIService = ShiftUIService;
    }
    internal async Task HandleMainMenu()
    {
        Console.Clear();
        var choice = _shiftView.ShowMainMenu();

        switch (choice)
        {
            case MainMenuOption.Create:
                await HandleCreate();
                break;
            case MainMenuOption.View:
                await HandleRead();
                break;
            case MainMenuOption.Update:
                await HandleUpdate();
                break;
            case MainMenuOption.Delete:
                await HandleDelete();
                break;
            case MainMenuOption.Exit:
                Environment.Exit(0);
                break;
        }
    }

    private async Task HandleCreate()
    {
        var shift = GetShift("Creating a shift");
        try
        {
            await _shiftUIService.Create(shift);
            Console.Clear();
            _shiftView.ReturnToMenu("Shift created successfully");
        } catch (Exception ex)
        {
            Console.Clear();
            _shiftView.ReturnToMenu(ex.Message);
        }
    }

    private Shift GetShift(string heading)
    {
        var error = "Follow the date and time format carefully";
        var shift = new Shift();
        bool flag = false;
        do
        {
            var msg = "Enter the [#66c6cc]start[/] date and time of the shift (yyyy-mm-dd hh:mm:ss)";
            if (flag) msg = $"[red]Shift cannot start after it ends[/]\n{msg}";
            var startTime = _shiftView.GetInput(heading, msg);
            while (!_shiftUIService.ValidDateTime(startTime))
            {
                msg = "Enter the [#66c6cc]start[/] date and time of the shift (yyyy-mm-dd hh:mm:ss)";
                startTime = _shiftView.GetInput(heading, msg, error);
            }
            shift.StartTime = _shiftUIService.FormatDateTime(startTime);

            msg = "Enter the [#66c6cc]end[/] date and time of the shift (yyyy-mm-dd hh:mm:ss)";
            var endTime = _shiftView.GetInput(heading, msg);
            while (!_shiftUIService.ValidDateTime(endTime))
            {
                endTime = _shiftView.GetInput(heading, msg, error);
            }
            shift.EndTime = _shiftUIService.FormatDateTime(endTime);
            flag = true;
        } while (DateTime.Parse(shift.StartTime) > DateTime.Parse(shift.EndTime));

        var duration = DateTime.Parse(shift.EndTime) - DateTime.Parse(shift.StartTime);
        shift.Duration = _shiftUIService.FormatDuration(duration);
        return shift;
    }

    private async Task HandleRead()
    {
        try
        {
            var shifts = await _shiftUIService.ReadAll();
            _shiftView.ShowShiftList(shifts);

        }
        catch (Exception ex)
        {
            _shiftView.ReturnToMenu(ex.Message);
        }
    }

    private async Task HandleUpdate()
    {
        try
        {
            var shifts = await _shiftUIService.ReadAll();
            var shiftToUpdate = _shiftView.ShowShiftPrompt(shifts);
            var updatedShift = GetShift("Updating a shift");
            updatedShift.id = shiftToUpdate.id;
            _shiftUIService.Update(shiftToUpdate.id, updatedShift);
            Console.Clear();
            _shiftView.ReturnToMenu("Shift updated successfully");
        } 
        catch (Exception ex)
        {
            _shiftView.ReturnToMenu(ex.Message);
        }
    }

    private async Task HandleDelete()
    {
        try
        {
            var shifts = await _shiftUIService.ReadAll();
            var shiftToDelete = _shiftView.ShowShiftPrompt(shifts);
            _shiftUIService.Delete(shiftToDelete.id);
            Console.Clear();
            _shiftView.ReturnToMenu($"Successfully deleted shift with id {shiftToDelete.id}");
        }
        catch (Exception ex)
        {
            _shiftView.ReturnToMenu(ex.Message);
        }
    }
}