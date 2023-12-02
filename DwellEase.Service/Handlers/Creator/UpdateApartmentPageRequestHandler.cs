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
    private readonly UpdateApartmentPageRequestToApartmentPageMapper _pageMapper;
    private readonly StringToGuidMapper _guidMapper;

    public UpdateApartmentPageRequestHandler(ApartmentPageService apartmentPageService, UpdateApartmentPageRequestToApartmentPageMapper mapper, StringToGuidMapper guidMapper)
    {
        _apartmentPageService = apartmentPageService;
        _pageMapper = mapper;
        _guidMapper = guidMapper;
    }

    public async Task<bool> Handle(UpdateApartmentPageRequest request, CancellationToken cancellationToken)
    {

        var guidId = _guidMapper.MapTo(request.PageId);
        var response = await _apartmentPageService.GetByIdAsync(guidId);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(response.Description);
        }

        var apartmentPage = _pageMapper.MapTo(request, response);
        await _apartmentPageService.EditAsync(apartmentPage);
        return true;
    }
}