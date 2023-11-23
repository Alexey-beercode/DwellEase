using DwellEase.Domain.Entity;
using MediatR;

namespace DwellEase.Service.Queries.Creator;

public class GetApartmentPagesByOwnerQuery : IRequest<List<ApartmentPage>>
{
    public string Id { get; set; }
}