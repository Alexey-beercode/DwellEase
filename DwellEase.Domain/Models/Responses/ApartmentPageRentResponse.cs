namespace DwellEase.Domain.Models.Responses;

public class ApartmentPageRentResponse
{
    public Guid UserId { get; set; }
    public Guid ApartmentPageId { get; set; }
    public string TimeRemaining { get; set; }

    public ApartmentPageRentResponse(Guid userId, Guid apartmentPageId, TimeSpan timeSpan)
    {
        UserId = userId;
        ApartmentPageId = apartmentPageId;
        TimeRemaining = $"Дней: {timeSpan.Days} Часов: {timeSpan.Hours} Минут: {timeSpan.Minutes}";
    }
}