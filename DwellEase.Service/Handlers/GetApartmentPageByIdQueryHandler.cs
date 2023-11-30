using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using MediatR;

namespace DwellEase.Service.Handlers;

public class GetApartmentPageByIdQueryHandler:IRequestHandler<GetApartmentPageByIdQuery, ApartmentPage>
{
    private readonly ApartmentPageService _apartmentPageService;

    public GetApartmentPageByIdQueryHandler(ApartmentPageService apartmentPageService)
    {
        _apartmentPageService = apartmentPageService;
    }
    
    public async Task<ApartmentPage> Handle(GetApartmentPageByIdQuery request, CancellationToken cancellationToken)
    {
        var response=await _apartmentPageService.GetByIdAsync(request.Id);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(response.Description);
        }
        return response.Data;
    }
}