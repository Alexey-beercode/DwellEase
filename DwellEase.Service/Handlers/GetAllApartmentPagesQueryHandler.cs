using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Responses;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using MediatR;

namespace DwellEase.Service.Handlers;

public class GetAllApartmentPagesQueryHandler : IRequestHandler<GetAllApartmentPagesQuery, List<ApartmentPageResponse>>
{
    private readonly ApartmentPageService _apartmentPageService;
    private readonly ApartmentPageToAprtmentPageResponseMapper _mapper;

    public GetAllApartmentPagesQueryHandler(ApartmentPageService apartmentPageService, ApartmentPageToAprtmentPageResponseMapper mapper)
    {
        _apartmentPageService = apartmentPageService;
        _mapper = mapper;
    }

    public async Task<List<ApartmentPageResponse>> Handle(GetAllApartmentPagesQuery request, CancellationToken cancellationToken)
    {
        var response= await _apartmentPageService.GetAllAsync();
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(response.Description);
        }

        var responses = new List<ApartmentPageResponse>();
        response.Data.ForEach(a => responses.Add(_mapper.MapTo(a)));
        return responses;
    }
}