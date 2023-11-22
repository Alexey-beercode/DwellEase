using System.Runtime.InteropServices;

namespace DwellEase.Domain.Models.Requests;

public class UpdateApartmentPageRequest
{
    public string PageId { get; set; }
    public decimal DailyPrice { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailableForPurchase { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Title { get; set; } = null!; 
}
    