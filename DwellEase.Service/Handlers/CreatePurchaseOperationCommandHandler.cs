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

    public CreatePurchaseOperationCommandHandler(ILogger<CreatePurchaseOperationCommandHandler> logger, ApartmentOperationService apartmentOperationService, CreatePurchaseOperationCommandToOperationMapper mapper)
    {
        _apartmentOperationService = apartmentOperationService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CreatePurchaseOperationCommand request, CancellationToken cancellationToken)
    {
       
        await _apartmentOperationService.CreateAsync(_mapper.MapTo(request));
        return true;
    }
    
}