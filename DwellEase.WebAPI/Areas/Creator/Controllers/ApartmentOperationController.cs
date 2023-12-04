using DwellEase.Domain.Entity;
using DwellEase.Service.Queries.Creator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DwellEase.WebAPI.Areas.Creator.Controllers;

[ApiController]
[Area("Creator")]
[Authorize(Policy = "CreatorArea")]
[Route("{area}/ApartmentOperation")]
public class ApartmentOperationController:ControllerBase
{
    private readonly IMediator _mediator;

    public ApartmentOperationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [SwaggerOperation("Gets a list of apartment operations by owner id")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<ApartmentOperation>))]
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