using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using MediatR;

namespace DwellEase.Service.Queries;

public class GetApartmentPageByIdQuery:IRequest<BaseResponse<ApartmentPage>>
{
    public Guid Id { get; set; }
    public GetApartmentPageByIdQuery(Guid result)
    {
        Id = result;
    }

  
}