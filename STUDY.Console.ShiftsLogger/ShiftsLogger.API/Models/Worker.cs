using System.ComponentModel.DataAnnotations;

namespace ShiftsLogger.API.Models;

public class Worker
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Name { get; set; } = string.Empty;
    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
