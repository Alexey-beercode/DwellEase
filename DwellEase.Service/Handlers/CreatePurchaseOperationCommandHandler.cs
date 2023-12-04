using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;
using DwellEase.Service.Mappers;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Handlers;

public class CreatePurchaseOperationCommandHandler:IRequestHandler<CreatePurchaseOperationCommand, bool>
{
    private readonly ApartmentOperationService _apartmentOperationService;
    private readonly CreatePurchaseOperationCommandToOperationMapper _mapper;
    private readonly ApartmentPageService _apartmentPageService;

    public CreatePurchaseOperationCommandHandler(ILogger<CreatePurchaseOperationCommandHandler> logger, ApartmentOperationService apartmentOperationService, CreatePurchaseOperationCommandToOperationMapper mapper, ApartmentPageService apartmentPageService)
    {
        _apartmentOperationService = apartmentOperationService;
        _mapper = mapper;
        _apartmentPageService = apartmentPageService;
    }

    public async Task<bool> Handle(CreatePurchaseOperationCommand request, CancellationToken cancellationToken)
    {
        var response = await _apartmentPageService.GetByIdAsync(request.ApartmentPageId);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(response.Description);
        }
        await _apartmentOperationService.CreateAsync(_mapper.MapTo(request));
        return true;
    }
    
}