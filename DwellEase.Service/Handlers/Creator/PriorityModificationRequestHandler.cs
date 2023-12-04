using System.Net;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Handlers.Creator;

public class PriorityModificationRequestHandler:IRequestHandler<PriorityModificationRequest,bool>
{
    private readonly SwitchPriorityRequestService _priorityRequestService;
    private readonly ApartmentPageService _apartmentPageService;
    private readonly PriorityModificationToSwitchMapper _mapper;
    private readonly StringToGuidMapper _guidMapper;

    public PriorityModificationRequestHandler(SwitchPriorityRequestService priorityRequestService, ILogger<PriorityModificationRequestHandler> logger, PriorityModificationToSwitchMapper mapper, ApartmentPageService apartmentPageService, StringToGuidMapper guidMapper)
    {
        _priorityRequestService = priorityRequestService;
        _mapper = mapper;
        _apartmentPageService = apartmentPageService;
        _guidMapper = guidMapper;
    }

    public async Task<bool> Handle(PriorityModificationRequest request, CancellationToken cancellationToken)
    {
        var switchRequest = _mapper.MapToSwitchPriorityRequest(request);
        var response = await _apartmentPageService.GetByIdAsync(_guidMapper.MapTo(request.ApartmentPageId));
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(response.Description);
        }
        await _priorityRequestService.Create(switchRequest);
        return true;
    }
}