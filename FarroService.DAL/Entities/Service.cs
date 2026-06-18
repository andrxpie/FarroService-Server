namespace FarroService.DAL.Entities;

public class Service
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid SpecializationId { get; set; }
    public Specialization? Specialization { get; set; }
}
