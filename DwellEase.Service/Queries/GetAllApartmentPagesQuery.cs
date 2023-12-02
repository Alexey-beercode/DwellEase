using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Responses;
using MediatR;

namespace DwellEase.Service.Queries;

public class GetAllApartmentPagesQuery : IRequest<BaseResponse<List<ApartmentPageRentResponse>>>, IRequest<List<ApartmentPageRentResponse>>, IRequest<List<ApartmentPageResponse>>
{
}