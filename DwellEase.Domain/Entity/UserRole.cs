using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DwellEase.Domain.Entity;

public class UserRole
{
    public ObjectId Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}