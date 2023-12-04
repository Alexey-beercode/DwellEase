using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Responses;
using MediatR;

namespace DwellEase.Service.Queries;

public class GetApartmentPageByIdQuery:IRequest<BaseResponse<ApartmentPage>>, IRequest<ApartmentPageRentResponse>, IRequest<ApartmentPageResponse>
{
    public string Id { get; set; }

  
}