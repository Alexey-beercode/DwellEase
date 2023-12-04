using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;
using DwellEase.Service.Mappers;
using DwellEase.Service.Services.Implementations;
using MediatR;

namespace DwellEase.Service.Handlers;

public class CreateRentOperationCommandHandler:IRequestHandler<CreateRentOperationCommand, bool>
{
    private readonly ApartmentOperationService _apartmentOperationService;
    private readonly CreateRentOperationCommandToOperationMapper _mapper;
    private readonly ApartmentPageService _apartmentPageService;

    public CreateRentOperationCommandHandler(ApartmentOperationService apartmentOperationService, CreateRentOperationCommandToOperationMapper mapper, ApartmentPageService apartmentPageService)
    {
        _apartmentOperationService = apartmentOperationService;
        _mapper = mapper;
        _apartmentPageService = apartmentPageService;
    }
    public async Task<bool> Handle(CreateRentOperationCommand request, CancellationToken cancellationToken)
    {
        var response = await _apartmentPageService.GetByIdAsync(request.ApartmentPageId);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(response.Description);
        }
        await _apartmentOperationService.CreateRentOperationAsync(_mapper.MapTo(request));
        return true;
    }
}