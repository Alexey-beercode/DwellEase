using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using MediatR;

namespace DwellEase.Service.Handlers;

public class GetAllApartmentPagesQueryHandler : IRequestHandler<GetAllApartmentPagesQuery, BaseResponse<List<ApartmentPage>>>
{
    private readonly ApartmentPageService _apartmentPageService;

    public GetAllApartmentPagesQueryHandler(ApartmentPageService apartmentPageService)
    {
        _apartmentPageService = apartmentPageService;
    }

    public async Task<BaseResponse<List<ApartmentPage>>> Handle(GetAllApartmentPagesQuery request, CancellationToken cancellationToken)
    {
        return await _apartmentPageService.GetApartmentPagesAsync();
    }
}