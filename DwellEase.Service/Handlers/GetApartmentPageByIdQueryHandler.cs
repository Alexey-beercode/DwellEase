using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using MediatR;

namespace DwellEase.Service.Handlers;

public class GetApartmentPageByIdQueryHandler:IRequestHandler<GetApartmentPageByIdQuery, BaseResponse<ApartmentPage>>
{
    private readonly ApartmentPageService _apartmentPageService;

    public GetApartmentPageByIdQueryHandler(ApartmentPageService apartmentPageService)
    {
        _apartmentPageService = apartmentPageService;
    }
    
    public async Task<BaseResponse<ApartmentPage>> Handle(GetApartmentPageByIdQuery request, CancellationToken cancellationToken)
    {
        return await _apartmentPageService.GetApartmentPageByIdAsync(request.Id);
    }
}