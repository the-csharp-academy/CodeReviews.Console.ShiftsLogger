using ShiftsLoggerApi.Data;
using ShiftsLoggerApi.DTOs;
using ShiftsLoggerApi.Models;

namespace ShiftsLoggerApi
{
    public class ShiftService
    {
        private readonly ShiftContext shiftContext;

        private readonly ShiftValidation shiftValidation;

        public ShiftService(ShiftContext shiftContextAssign, ShiftValidation shiftValidationAssign)
        {
            shiftContext = shiftContextAssign;
            shiftValidation = shiftValidationAssign;
        }

        public ShiftResult GetById(int id)
        {
            ShiftResult shiftResult = new ShiftResult();

            if (!shiftValidation.IsIdCorrect(id))
            {
                shiftResult.ShiftError = ShiftError.InvalidId;
                return shiftResult;
            }
            var shift = shiftContext.Shifts.FirstOrDefault(shift => shift.Id == id);
            if (shift == null)
            {
                shiftResult.ShiftError = ShiftError.NotFound;
                return shiftResult;
            }
            shiftResult.ShiftError = ShiftError.None;
            shiftResult.Shift = shift;
            shiftResult.Shift.Duration = shiftResult.Shift.EndDate - shiftResult.Shift.StartDate;

            return shiftResult;
        }

        public IEnumerable<ShiftsLog> GetAll()
        {
            var shiftslist = shiftContext.Shifts;
            foreach (ShiftsLog shift in shiftslist)
            {
                shift.Duration = shift.EndDate - shift.StartDate;
            }
            return shiftslist;
        }

        public ShiftResult Create(ShiftDto shiftDto)
        {
            ShiftResult shiftResult = new ShiftResult();

            if (!shiftValidation.IsDateCorrect(shiftDto.StartDate, shiftDto.EndDate))
            {
                shiftResult.ShiftError = ShiftError.InvalidDate;
                return shiftResult;
            }
            var newShift = new ShiftsLog
            {
                WorkerId = shiftDto.WorkerId,
                StartDate = shiftDto.StartDate,
                EndDate = shiftDto.EndDate
            };
            shiftContext.Shifts.Add(newShift);
            shiftContext.SaveChanges();

            shiftResult.ShiftError = ShiftError.None;
            shiftResult.Shift = newShift;
            shiftResult.Shift.Duration = shiftResult.Shift.EndDate - shiftResult.Shift.StartDate;

            return shiftResult;
        }

        public ShiftResult Put(int id, ShiftDto shiftDto)
        {
            ShiftResult shiftResult = new ShiftResult();

            if (!shiftValidation.IsIdCorrect(id))
            {
                shiftResult.ShiftError = ShiftError.InvalidId;
                return shiftResult;
            }
            if (!shiftValidation.IsDateCorrect(shiftDto.StartDate, shiftDto.EndDate))
            {
                shiftResult.ShiftError = ShiftError.InvalidDate;
                return shiftResult;
            }
            var shiftToUpdate = shiftContext.Shifts.FirstOrDefault(shift => shift.Id == id);
            if (shiftToUpdate == null)
            {
                shiftResult.ShiftError = ShiftError.NotFound;
                return shiftResult;
            }
            shiftToUpdate.WorkerId = shiftDto.WorkerId;
            shiftToUpdate.StartDate = shiftDto.StartDate;
            shiftToUpdate.EndDate = shiftDto.EndDate;
            shiftToUpdate.Duration = shiftDto.EndDate - shiftDto.StartDate;

            shiftContext.SaveChanges();

            shiftResult.ShiftError = ShiftError.None;
            shiftResult.Shift = shiftToUpdate;
            return shiftResult;
        }

        public ShiftResult Delete(int id)
        {
            ShiftResult shiftResult = new ShiftResult();

            if (!shiftValidation.IsIdCorrect(id))
            {
                shiftResult.ShiftError = ShiftError.InvalidId;
                return shiftResult;
            }
            var newShift = shiftContext.Shifts.FirstOrDefault(shift => shift.Id == id);
            if (newShift == null)
            {
                shiftResult.ShiftError = ShiftError.NotFound;
                return shiftResult;
            }

            shiftContext.Shifts.Remove(newShift);
            shiftContext.SaveChanges();

            shiftResult.ShiftError = ShiftError.None;
            shiftResult.Shift = newShift;
            return shiftResult;
        }
    }
}