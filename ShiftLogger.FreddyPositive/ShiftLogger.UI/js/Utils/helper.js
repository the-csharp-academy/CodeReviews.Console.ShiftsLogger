export class ShiftLoggerHelper {
    RenderEmployeeDropDown(employeeList) {
        const employeeDropdownOption = $("#EmployeeListDdl ul");
        employeeDropdownOption.empty();
        employeeList.forEach(emp => {
            employeeDropdownOption.append(
                `<li data-empid="${emp.id}"class="dropdown-item">${emp.name}</li>`
            )
        });
    }

    RenderShiftLogTable(shiftLog) {
        const shiftLogTable = $(".sl-shiftlog-table tBody");
        shiftLogTable.empty();
        var RowNo = 1;
        shiftLog.forEach(log => {
            shiftLogTable.append(
                `<tr>
              <td>${RowNo++}</td>
              <td>${log.name}</td>
              <td>${this.formatShiftStatus(log.shiftStatus)}</td>
              <td>${this.formatHours(log.totalWorkingHours)}</td>
            </tr>`
            )
        });
    }

    formatShiftStatus(shiftStatus) {
        return (shiftStatus == 1) ? "Active" : "In-Active"
    }

    formatHours(totalWorkingHours) {
        if (totalWorkingHours == null || isNaN(totalWorkingHours)) return "-";
        const totalMinutes = Math.round(totalWorkingHours * 60);
        const hours = Math.floor(totalMinutes / 60);
        const minutes = totalMinutes % 60;

        if (hours === 0) return `${minutes} min`;
        if (minutes === 0) return `${hours} hr`;
        return `${hours} hr ${minutes} min`;
    }
    ValidEmployeeId(employeeId) {
        if (employeeId == 0 || employeeId == "" || employeeId == null || employeeId == undefined) {
            Swal.fire({
                title: "Error!",
                text: "Please Choose a Employee to proceed",
                icon: "error",
                confirmButtonText: "OK"
            });
            return false;
        }
        return true;
    }
}