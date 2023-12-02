using DwellEase.Domain.Models.Identity;
using DwellEase.Domain.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DwellEase.WebAPI.Areas.Creator.Controllers;

[ApiController]
[Area("Creator")]
[Route("{area}/SwitchPriorityRequest")]
[Authorize(Policy = "CreatorArea")]
public class SwitchPriorityRequestController:ControllerBase
{
    private readonly IMediator _mediator;

    public SwitchPriorityRequestController(ILogger<SwitchPriorityRequestController> logger, IMediator mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation("Create switchpriority request")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200)]
    [HttpPost("CreateRequest")]
    public async Task<IActionResult> CreateSwitchPriorityRequest(PriorityModificationRequest request)
    {
        try
        {
            await _mediator.Send(request);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}