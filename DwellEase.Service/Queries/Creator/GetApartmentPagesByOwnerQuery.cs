using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Responses;
using MediatR;

namespace DwellEase.Service.Queries.Creator;

public class GetApartmentPagesByOwnerQuery : IRequest<List<ApartmentPageRentResponse>>, IRequest<List<ApartmentPageResponse>>
{
    public string Id { get; set; }
}