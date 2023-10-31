using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace DwellEase.Service.Commands;

public class CreateApartmentPageCommand
{
    public decimal DaylyPrice { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailableForPurchase { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string OwnerId { get; set; }
    public string Title { get; set; } = null!;
    public int Rooms { get; set; }
    public double Area { get; set; }
    public string Street { get; set; } = null!;
    public int HouseNumber { get; set; } 
    public string City { get; set; } = null!;
    public string Building { get; set; } = null!;
    public string ApartmentType { get; set; }
    public List<IFormFile> Images { get; set; } = null!;
}