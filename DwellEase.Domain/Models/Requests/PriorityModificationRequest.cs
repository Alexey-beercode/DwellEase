namespace DwellEase.Domain.Models.Requests;

public class PriorityModificationRequest
{
    public string NewType { get; set; }
    public string UserId { get; set; }
    public string ApartmentPageId { get; set; }
}