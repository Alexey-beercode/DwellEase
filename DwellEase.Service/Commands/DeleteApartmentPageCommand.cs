using MediatR;

namespace DwellEase.Service.Queries.Creator;

public class DeleteApartmentPageCommand:IRequest<bool>
{
    public string Id { get; set; }
}