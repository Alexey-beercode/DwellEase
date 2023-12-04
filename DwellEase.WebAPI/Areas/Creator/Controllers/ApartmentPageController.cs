using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Commands;
using DwellEase.Service.Queries.Creator;
using DwellEase.Shared;
using DwellEase.WebAPI.Validators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DwellEase.WebAPI.Areas.Creator.Controllers;

[ApiController]
[Area("Creator")]
[Route("{area}/ApartmentPage")]
[Authorize(Policy = "CreatorArea")]
public class ApartmentPageController : ControllerBase
{
    private readonly ILogger<ApartmentPageController> _logger;
    private readonly IMediator _mediator;

    public ApartmentPageController(ILogger<ApartmentPageController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [SwaggerOperation("Create apartment page")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200)]
    [HttpPost("CreateApartmentPage")]
    public async Task<IActionResult> CreateApartmentPage([FromBody] CreateApartmentPageRequest request)
    {
        var validateResult =
            GeneralValidator.Validate(new CreateApartmentPageRequestValidator(), request);
        if (validateResult.Length != 0)
        {
            return BadRequest(validateResult);
        }
        try
        {
            await _mediator.Send(request);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        _logger.LogInformation("Successfully create Apartment page");
        return Ok();
    }

    [SwaggerOperation("Update apartment page")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200)]
    [HttpPut("UpdateApartmentPage")]
    public async Task<IActionResult> UpdateApartmentPage([FromBody] UpdateApartmentPageRequest request)
    {
        var validateResult = GeneralValidator.Validate(new UpdateApartmentPageRequestValidator(), request);
        if (validateResult.Length != 0)
        {
            return BadRequest(validateResult);
        }

        try
        {
            await _mediator.Send(request);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [SwaggerOperation("Delete aprtment page by id")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200)]
    [HttpDelete("DeleteApartmentPage")]
    public async Task<IActionResult> DeleteApartmentPage([FromBody] string id)
    {
        try
        {
            await _mediator.Send(new DeleteApartmentPageCommand() { Id = id });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [SwaggerOperation("Gets a list of apartment pages by owener id")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<ApartmentPage>))]
    [HttpGet("GetApartmentPagesByOwner/{id}")]
    public async Task<IActionResult> GetApartmentPagesByOwner(string id)
    {
        try
        {
            var pages = await _mediator.Send(new GetApartmentPagesByOwnerQuery() { Id = id });
            return Ok(pages);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}