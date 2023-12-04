using DwellEase.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.Domain.Models.Responses;

public class ApartmentPageResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ApprovalStatus { get; set; }
    public string PriorityType { get; set; }
    public string Status { get; set; }
    public decimal DaylyPrice { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailableForPurchase { get; set; }
    public Apartment Apartment { get; set; } = null!;
    public PhoneNumber PhoneNumber { get; set; } = null!;
    public Guid OwnerId { get; set; }
    public List<FileContentResult> Images { get; set; }
    public DateOnly Date { get; set; }
}