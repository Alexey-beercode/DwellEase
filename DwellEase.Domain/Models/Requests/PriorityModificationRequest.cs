using MediatR;

namespace DwellEase.Domain.Models.Requests;

public class PriorityModificationRequest:IRequest<bool>
{
    public string NewType { get; set; }
    public string UserId { get; set; }
    public string ApartmentPageId { get; set; }
}