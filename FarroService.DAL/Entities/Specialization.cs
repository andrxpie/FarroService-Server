namespace FarroService.DAL.Entities;

public class Specialization
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Service> Services { get; set; } = new List<Service>();
    public ICollection<ApplicationUser> Masters { get; set; } = new List<ApplicationUser>();
}
