using System.Net;
using DwellEase.Service.Queries.Creator;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Handlers.Creator;

public class DeleteApartmentPageCommandHandler:IRequestHandler<DeleteApartmentPageCommand,bool>
{
    private readonly ILogger<DeleteApartmentPageCommandHandler> _logger;
    private readonly ApartmentPageService _apartmentPageService;

    public DeleteApartmentPageCommandHandler(ApartmentPageService apartmentPageService, ILogger<DeleteApartmentPageCommandHandler> logger)
    {
        _apartmentPageService = apartmentPageService;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteApartmentPageCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.Id,out Guid guidId))
        {
            throw new ("OwnerId is not valid");
        }

        var response = await _apartmentPageService.GetByIdAsync(guidId);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new(response.Description);
        }
        await _apartmentPageService.DeleteAsync(guidId);
        return true;
    }
}