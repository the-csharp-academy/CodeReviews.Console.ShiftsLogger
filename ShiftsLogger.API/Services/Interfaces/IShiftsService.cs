public interface IShiftsService
{
    Task<ICollection<Shift>> GetAllAsync();
    Task<Shift?> GetByIdAsync(int id);
    Task<Shift?> CreateAsync(Shift dto);
    Task<ShiftUpdateResult> UpdateAsync(int id, Shift dto);
    Task<bool> DeleteAsync(int id);
}