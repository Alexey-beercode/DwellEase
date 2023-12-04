using MediatR;

namespace DwellEase.Domain.Models.Requests;

public class UpdateApprovalStatusRequest:IRequest<bool>
{
    public string ApartmentPageId { get; set; }
    public string NewStatus { get; set; }
}