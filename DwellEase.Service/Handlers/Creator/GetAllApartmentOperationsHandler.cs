using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;
using DwellEase.Service.Queries.Creator;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Handlers.Creator;

public class GetAllApartmentOperationsQueryHandler:IRequestHandler<GetAllApartmentOperationsQuery, List<ApartmentOperation>>
{
    private readonly ILogger<GetAllApartmentOperationsQueryHandler> _logger;
    private readonly ApartmentOperationService _apartmentOperationService;
    
    public GetAllApartmentOperationsQueryHandler(ILogger<GetAllApartmentOperationsQueryHandler> logger, ApartmentOperationService apartmentOperationService)
    {
        _logger = logger;
        _apartmentOperationService = apartmentOperationService;
    }

    public async Task<List<ApartmentOperation>> Handle(GetAllApartmentOperationsQuery request, CancellationToken cancellationToken)
    {
        var operationResponse = await _apartmentOperationService.GetAllAsync();
        if (operationResponse.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(operationResponse.Description);
        }
        return operationResponse.Data.Where(a => a.UserId == Guid.Parse(request.Id)).ToList();
    }
}