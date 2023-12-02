using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DwellEase.WebAPI.Areas.Admin.Controllers;

[ApiController]
[Area("Admin")]
[Route("{area}/SwitchPriorityRequest")]
[Authorize(Policy = "AdminArea")]
public class SwitchPriorityRequestController:ControllerBase
{
    private readonly IMediator _mediator;
    private readonly SwitchPriorityRequestService _priorityRequestService;

    public SwitchPriorityRequestController(IMediator mediator, SwitchPriorityRequestService priorityRequestService)
    {
        _mediator = mediator;
        _priorityRequestService = priorityRequestService;
    }

    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200)]
    [HttpPut("ApprovePriorityRequest")]
    public async Task<IActionResult> ApprovePriorityRequest([FromBody] string id)
    {
       try
       {
           await _mediator.Send(new ApprovePriorityRequestCommand() { Id = id });
           return Ok();
       }
       catch (Exception e)
       {
           return BadRequest(e.Message);
       }
    }

    [SwaggerOperation("Gets a list of switch priority requests")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<SwitchPriorityRequest>))]
    [HttpGet("GetAllPriorityRequests")]
    public async Task<IActionResult> GetAllPriorityRequests()
    {
        var response = await _priorityRequestService.GetAllAsync();
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            return BadRequest(response.Description);
        }
        
        return Ok(response.Data);
    }
}