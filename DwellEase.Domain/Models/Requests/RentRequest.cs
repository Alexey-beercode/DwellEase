using DwellEase.Domain.Enum;

namespace DwellEase.Domain.Models.Requests;

public class RentRequest
{
    public string UserId { get; set; }
    public string ApartmentPageId { get; set; }
    public string RentalPeriod { get; set; }
}