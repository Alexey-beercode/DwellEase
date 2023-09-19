using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using MediatR;

namespace DwellEase.Service.Queries;

public class GetAllApartmentPagesQuery : IRequest<BaseResponse<List<ApartmentPage>>>
{
}