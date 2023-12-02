using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Responses;
using DwellEase.Service.Queries.Creator;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using MediatR;

namespace DwellEase.Service.Handlers.Creator;

public class GetApartmentPagesByOwnerQueryHandler:IRequestHandler<GetApartmentPagesByOwnerQuery, List<ApartmentPageResponse>>
{
    private readonly ApartmentPageService _apartmentPageService;
    private readonly UserService _userService;
    private readonly StringToGuidMapper _guidMapper;
    private readonly ApartmentPageToAprtmentPageResponseMapper _pageResponseMapper;

    public GetApartmentPagesByOwnerQueryHandler(ApartmentPageService apartmentPageService, UserService userService, StringToGuidMapper mapper, ApartmentPageToAprtmentPageResponseMapper pageResponseMapper)
    {
        _apartmentPageService = apartmentPageService;
        _userService = userService;
        _guidMapper = mapper;
        _pageResponseMapper = pageResponseMapper;
    }

    public async Task<List<ApartmentPageResponse>> Handle(GetApartmentPagesByOwnerQuery request, CancellationToken cancellationToken)
    {
        var guidUserId = _guidMapper.MapTo(request.Id);
        var userResponse = await _userService.GetByIdAsync(guidUserId);
        if (userResponse.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(userResponse.Description);
        }

        var pagesResponse = await _apartmentPageService.GetByOwnerAsync(guidUserId);
        if (pagesResponse.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(pagesResponse.Description);
        }

        var responses = new List<ApartmentPageResponse>();
        pagesResponse.Data.ForEach(a => responses.Add(_pageResponseMapper.MapTo(a)));
        return responses;

    }
}