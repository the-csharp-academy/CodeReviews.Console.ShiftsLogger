
import { ShiftLoggerComponent } from "./Components/ShiftLoggerComponent.js";
import { ApiService } from "./API/APIService.js";
import { ShiftLoggerHelper } from "./Utils/helper.js"

const apiService = new ApiService("https://localhost:7210/api/ShiftLogger/");

$(document).ready(() => {

    EmployeeDropDown();

    ShiftLogTable();

    $(document).on("click", "#sl-shift-in", async function () {
        const employeeId = $("#EmployeeListDdl").data("selected-empid");
        const shiftLoggerHelper = new ShiftLoggerHelper();
        var isValidEmployeeId = shiftLoggerHelper.ValidEmployeeId(employeeId);
        if (isValidEmployeeId) {
            $('#sl-shift-in').prop('disabled', true);
            $('#sl-shift-out').prop('disabled', false);
            const shiftLogger = new ShiftLoggerComponent(apiService);
            await shiftLogger.ShiftIn(employeeId);
            ShiftLogTable();
        }
    });

    $(document).on("click", "#sl-shift-out", async function () {
        const employeeId = $("#EmployeeListDdl").data("selected-empid");
        const shiftLoggerHelper = new ShiftLoggerHelper();
        var isValidEmployeeId = shiftLoggerHelper.ValidEmployeeId(employeeId);
        if (isValidEmployeeId) {
            $('#sl-shift-in').prop('disabled', false);
            $('#sl-shift-out').prop('disabled', true);
            const shiftLogger = new ShiftLoggerComponent(apiService);
            await shiftLogger.ShiftOut(employeeId);
            ShiftLogTable();
        }
    });

    $(document).on("click", "#EmployeeListDdl ul li", function () {
        const empId = $(this).data("empid");
        const empName = $(this).text();
        $("#EmployeeListDdl button").text(empName);
        $("#EmployeeListDdl").data("selected-empid", empId);
        GetEmployeeShiftStatus(empId);
    });
});

async function EmployeeDropDown() {
    const shiftLogger = new ShiftLoggerComponent(apiService);
    const employeeList = await shiftLogger.GetEmployees();
    const shiftLoggerHelper = new ShiftLoggerHelper();
    await shiftLoggerHelper.RenderEmployeeDropDown(employeeList)
}

async function ShiftLogTable() {
    const shiftLogger = new ShiftLoggerComponent(apiService);
    const shiftLog = await shiftLogger.GetShiftLog();
    const shiftLoggerHelper = new ShiftLoggerHelper();
    await shiftLoggerHelper.RenderShiftLogTable(shiftLog)
}

async function GetEmployeeShiftStatus(empId) {
    const shiftLogger = new ShiftLoggerComponent(apiService);
    var isEmployeeShiftActive = await shiftLogger.GetEmployeeShiftStatus(empId)
    if (isEmployeeShiftActive == true) {
        $('#sl-shift-out').prop('disabled', false);
        $('#sl-shift-in').prop('disabled', true);
    }
    else {
        $('#sl-shift-out').prop('disabled', true);
        $('#sl-shift-in').prop('disabled', false);
    }
}