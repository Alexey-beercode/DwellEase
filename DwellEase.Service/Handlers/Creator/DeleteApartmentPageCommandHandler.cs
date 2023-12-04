using System.Net;
using DwellEase.Service.Commands;
using DwellEase.Service.Queries.Creator;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using MediatR;

namespace DwellEase.Service.Handlers.Creator;

public class DeleteApartmentPageCommandHandler:IRequestHandler<DeleteApartmentPageCommand,bool>
{
    private readonly ApartmentPageService _apartmentPageService;
    private readonly StringToGuidMapper _mapper;

    public DeleteApartmentPageCommandHandler(ApartmentPageService apartmentPageService, StringToGuidMapper mapper)
    {
        _apartmentPageService = apartmentPageService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteApartmentPageCommand request, CancellationToken cancellationToken)
    {
        var guidId = _mapper.MapTo(request.Id);
        var response = await _apartmentPageService.GetByIdAsync(guidId);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new(response.Description);
        }
        await _apartmentPageService.DeleteAsync(guidId);
        return true;
    }
}