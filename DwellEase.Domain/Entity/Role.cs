using Microsoft.AspNetCore.Identity;

namespace DwellEase.Domain.Entity;

public class Role : IdentityRole<Guid>
{
    public new Guid Id { get; set; }
    public string RoleName { get; set; }
    public string NormalizedRoleName { get; set; }
    public Guid UserId { get; set; }
}