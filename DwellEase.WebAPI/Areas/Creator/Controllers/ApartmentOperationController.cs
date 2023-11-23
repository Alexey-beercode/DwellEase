using DwellEase.Service.Queries.Creator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Creator.Controllers;

[Area("Creator")]
[Authorize(Policy = "CreatorArea")]
[Route("ApartmentOperation")]
public class ApartmentOperationController:ControllerBase
{
    private readonly IMediator _mediator;

    public ApartmentOperationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetOperationsByPagesOwner")]
    public async Task<IActionResult> GetOperationsByPagesOwner(string id)
    {
        try
        {
            var operations = await _mediator.Send(new GetOperationsByPagesOwnerQuery() { Id = id });
            return Ok(operations);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}