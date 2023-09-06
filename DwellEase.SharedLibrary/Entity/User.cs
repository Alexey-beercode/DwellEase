using DwellEase.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace SharedLibrary.Entity;

public class User:IdentityUser<Guid>
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public PhoneNumber PhoneNumber { get; set; }
    public Role Role { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    
    public override string ToString()
        => UserName ?? string.Empty;
 
}