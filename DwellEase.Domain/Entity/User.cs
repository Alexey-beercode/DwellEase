using DwellEase.Domain.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoFramework.AspNetCore.Identity;

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
    
    public override string ToString()
        => UserName ?? string.Empty;
 
}