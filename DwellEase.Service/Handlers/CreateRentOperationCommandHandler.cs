using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;
using DwellEase.Service.Mappers;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Handlers;

public class CreateRentOperationCommandHandler:IRequestHandler<CreateRentOperationCommand, bool>
{
    private readonly ApartmentOperationService _apartmentOperationService;
    private readonly CreateRentOperationCommandToOperationMapper _mapper;

    public CreateRentOperationCommandHandler(ILogger<CreateRentOperationCommandHandler> logger, ApartmentOperationService apartmentOperationService, CreateRentOperationCommandToOperationMapper mapper)
    {
        _apartmentOperationService = apartmentOperationService;
        _mapper = mapper;
    }
    public async Task<bool> Handle(CreateRentOperationCommand request, CancellationToken cancellationToken)
    {
        await _apartmentOperationService.CreateRentOperationAsync(_mapper.MapTo(request));
        return true;
    }
}