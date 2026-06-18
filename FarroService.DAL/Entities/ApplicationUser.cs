using Microsoft.AspNetCore.Identity;

namespace FarroService.DAL.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
}
