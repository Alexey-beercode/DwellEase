using Microsoft.AspNetCore.Http;

namespace DwellEase.Domain.Models.Requests;

public class CreateApartmentPageRequest
{
    public decimal DaylyPrice { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailableForPurchase { get; set; }
    public Apartment Apartment { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string OwnerId { get; set; }
    public List<IFormFile> Images { get; set; } = null!;
}