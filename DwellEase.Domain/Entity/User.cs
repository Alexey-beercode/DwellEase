using DwellEase.Domain.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DwellEase.Domain.Entity;

public class User
{
    [BsonId]
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string NormalizedUserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public PhoneNumber PhoneNumber { get; set; }
    public Role Role { get; set; } = new Role(){RoleName = "Resident"};
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string PasswordSalt { get; set; }
    
 
}