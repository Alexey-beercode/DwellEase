using System.Net;
using DwellEase.Service.Commands;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DwellEase.Service.Handlers.Admin;

public class ApprovePriorityRequestCommandHandler:IRequestHandler<ApprovePriorityRequestCommand,bool>
{
    private readonly SwitchPriorityRequestService _priorityRequestService;
    private readonly StringToGuidMapper _mapper;

    public ApprovePriorityRequestCommandHandler(SwitchPriorityRequestService priorityRequestService, StringToGuidMapper mapper)
    {
        _priorityRequestService = priorityRequestService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(ApprovePriorityRequestCommand request, CancellationToken cancellationToken)
    {
        var response = await _priorityRequestService.Approve(_mapper.MapTo(request.Id));
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new(response.Description);
        }
        return true;
    }
}