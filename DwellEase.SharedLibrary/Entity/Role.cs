
using Microsoft.AspNetCore.Identity;

namespace SharedLibrary.Entity;
public class Role:IdentityRole<Guid>
{
    public Guid Id { get; set; }
    public string RoleName { get; set; }
    public string NormalizedRoleName { get; set; }
    public Guid UserId { get; set; }
}