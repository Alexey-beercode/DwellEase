using MediatR;

namespace DwellEase.Service.Commands;

public class ApprovePriorityRequestCommand:IRequest<bool>
{
    public string Id { get; set; }
}