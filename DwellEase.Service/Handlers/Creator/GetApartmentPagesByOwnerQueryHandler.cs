using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Service.Queries.Creator;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Handlers.Creator;

public class GetApartmentPagesByOwnerQueryHandler:IRequestHandler<GetApartmentPagesByOwnerQuery, List<ApartmentPage>>
{
    private readonly ApartmentPageService _apartmentPageService;
    private readonly ILogger<GetOperationsByPagesOwnerQueryHandler> _logger;
    private readonly UserService _userService;

    public GetApartmentPagesByOwnerQueryHandler(ApartmentPageService apartmentPageService, ILogger<GetOperationsByPagesOwnerQueryHandler> logger, UserService userService)
    {
        _apartmentPageService = apartmentPageService;
        _logger = logger;
        _userService = userService;
    }

    public async Task<List<ApartmentPage>> Handle(GetApartmentPagesByOwnerQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.Id,out var guidUserId))
        {
            throw new Exception("Invalid Id");
        }

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

        return pagesResponse.Data;
    }
}