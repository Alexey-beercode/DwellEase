using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DwellEase.Domain.Entity;

public class ApartmentPage
{
    [BsonId] 
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public ListingApprovalStatus ApprovalStatus { get; set; } = ListingApprovalStatus.Pending;
    public PriorityType PriorityType { get; set; }
    public ApartmentStatus Status { get; set; }
    public decimal DaylyPrice { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailableForPurchase { get; set; }
    public Apartment Apartment { get; set; } = null!;
    public PhoneNumber PhoneNumber { get; set; } = null!;
    public Guid OwnerId { get; set; }
    public List<Image> Images { get; set; } = null!;
    public DateOnly Date { get; set; }
}