using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Queries.Creator;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using MediatR;

namespace DwellEase.Service.Handlers.Creator;

public class UpdateApartmentPageRequestHandler:IRequestHandler<UpdateApartmentPageRequest,bool>
{
    private readonly ApartmentPageService _apartmentPageService;
    private readonly UpdateApartmentPageRequestToApartmentPageMapper _mapper;

    public UpdateApartmentPageRequestHandler(ApartmentPageService apartmentPageService, UpdateApartmentPageRequestToApartmentPageMapper mapper)
    {
        _apartmentPageService = apartmentPageService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateApartmentPageRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.PageId,out Guid guidId))
        { 
            throw new Exception("OwnerId is not valid");
        }

        var response = await _apartmentPageService.GetByIdAsync(guidId);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(response.Description);
        }

        var apartmentPage = _mapper.MapTo(request, response);
        await _apartmentPageService.EditAsync(apartmentPage);
        return true;
    }
}