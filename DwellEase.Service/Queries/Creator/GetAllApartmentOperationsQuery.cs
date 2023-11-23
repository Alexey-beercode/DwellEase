using DwellEase.Domain.Entity;
using MediatR;

namespace DwellEase.Service.Queries.Creator;

public class GetAllApartmentOperationsQuery : IRequest<List<ApartmentOperation>>
{
    public string Id { get; set; }
}