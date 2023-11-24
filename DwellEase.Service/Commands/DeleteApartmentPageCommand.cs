using MediatR;

namespace DwellEase.Service.Commands;

public class DeleteApartmentPageCommand:IRequest<bool>
{
    public string Id { get; set; }
}