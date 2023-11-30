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
    private readonly ILogger<PriorityModificationRequestHandler> _logger;
    private readonly PriorityModificationToSwitchMapper _mapper;

    public PriorityModificationRequestHandler(SwitchPriorityRequestService priorityRequestService, ILogger<PriorityModificationRequestHandler> logger, PriorityModificationToSwitchMapper mapper)
    {
        _priorityRequestService = priorityRequestService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<bool> Handle(PriorityModificationRequest request, CancellationToken cancellationToken)
    {
        var switchRequest = _mapper.MapToSwitchPriorityRequest(request);
        await _priorityRequestService.Create(switchRequest);
        return true;
    }
}