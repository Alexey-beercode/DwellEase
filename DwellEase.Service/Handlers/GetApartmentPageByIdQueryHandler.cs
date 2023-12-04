using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Responses;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using MediatR;

namespace DwellEase.Service.Handlers;

public class GetApartmentPageByIdQueryHandler:IRequestHandler<GetApartmentPageByIdQuery, ApartmentPageResponse>
{
    private readonly ApartmentPageService _apartmentPageService;
    private readonly ApartmentPageToAprtmentPageResponseMapper _apartmentPageToAprtmentPageResponseMapper;
    private readonly StringToGuidMapper _guidMapper;

    public GetApartmentPageByIdQueryHandler(ApartmentPageService apartmentPageService, ApartmentPageToAprtmentPageResponseMapper apartmentPageToAprtmentPageResponseMapper, StringToGuidMapper guidMapper)
    {
        _apartmentPageService = apartmentPageService;
        _apartmentPageToAprtmentPageResponseMapper = apartmentPageToAprtmentPageResponseMapper;
        _guidMapper = guidMapper;
    }
    
    public async Task<ApartmentPageResponse> Handle(GetApartmentPageByIdQuery request, CancellationToken cancellationToken)
    {
        
        var response=await _apartmentPageService.GetByIdAsync(_guidMapper.MapTo(request.Id));
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(response.Description);
        }
        return _apartmentPageToAprtmentPageResponseMapper.MapTo(response.Data);
    }
}