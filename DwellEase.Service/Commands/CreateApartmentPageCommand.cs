using DwellEase.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace DwellEase.Service.Commands;

public class CreateApartmentPageCommand
{
    public decimal DaylyPrice { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailableForPurchase { get; set; }
    public Apartment Apartment { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string OwnerId { get; set; }
    public List<Image> Images { get; set; } = null!;
}