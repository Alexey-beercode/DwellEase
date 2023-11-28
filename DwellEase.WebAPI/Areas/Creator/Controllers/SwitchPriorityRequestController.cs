﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Creator.Controllers;

[Area("Creator")]
[Route("SwitchPriorityRequest")]
[Authorize(Policy = "CreatorArea")]
public class SwitchPriorityRequestController:ControllerBase
{
    private readonly ILogger<SwitchPriorityRequestController> _logger;
    private readonly IMediator _mediator;

    public SwitchPriorityRequestController(ILogger<SwitchPriorityRequestController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpPost("CreateRequest")]
    public async Task<IActionResult> CreateSwitchPriorityRequest()
}