import { ApiService } from "../API/APIService.js";

export class ShiftLoggerComponent {
    constructor(apiService) {
        this.apiService = apiService;
    }
    async ShiftIn(employeeId) {
        try {
            const result = await this.apiService.post('shift-in', { employeeId });
            Swal.fire({
                title: "Success!",
                text: result.data,
                icon: "success",
                confirmButtonText: "OK"
            });
        }
        catch (error) {
            Swal.fire({
                title: "Error!",
                text: error.details || "Something went wrong during shift-in.",
                icon: "error",
                confirmButtonText: "Try Again"
            });
        }
    }
    async ShiftOut(employeeId) {
        try {
            const result = await this.apiService.post('shift-out', { employeeId });
            Swal.fire({
                title: "Success!",
                text: result.data,
                icon: "success",
                confirmButtonText: "OK"
            });
        }
        catch (error) {
            Swal.fire({
                title: "Error!",
                text: error.details || "Something went wrong during shift-out.",
                icon: "error",
                confirmButtonText: "Try Again"
            });
        }
    }
    async GetEmployees() {
        {
            try {
                const result = await this.apiService.get('employeelist');
                return result.data;
            }
            catch (error) {
                Swal.fire({
                    title: "Error!",
                    text: error.details || "No Employees to Display.",
                    icon: "error",
                    confirmButtonText: "Try Again"
                });
            }
        }
    }
    async GetShiftLog() {
        {
            try {
                const result = await this.apiService.get('shiftlog');
                return result.data;
            }
            catch (error) {
                Swal.fire({
                    title: "Error!",
                    text: error.details || "No Shift Log to display.",
                    icon: "error",
                    confirmButtonText: "Try Again"
                });
            }
        }
    }
    async GetEmployeeShiftStatus(employeeId) {
        const result = await this.apiService.getValue('checkshiftstatus', { employeeId });
        return result.data;
    }
}
