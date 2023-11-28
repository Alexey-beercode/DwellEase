using System.Text;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Commands;
using DwellEase.Service.Queries.Creator;
using DwellEase.WebAPI.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Creator.Controllers;

[ApiController]
[Area("Creator")]
[Route("{area}/ApartmentPage")]
[Authorize(Policy = "CreatorArea")]
public class ApartmentPageController:ControllerBase
{
    private readonly ILogger<ApartmentPageController> _logger;
    private readonly IMediator _mediator;

    public ApartmentPageController(ILogger<ApartmentPageController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    private StringBuilder Validate<T>(AbstractValidator<T> validator, T model)
    {
        var validateResult = validator.ValidateAsync(model).Result;
        if (!validateResult.IsValid)
        {
            var errors =new StringBuilder();
            validateResult.Errors.ForEach(a => errors.Append(a.ErrorMessage).Append("\n"));
            return errors;
        }
        return new StringBuilder();
    }
    
    [HttpPost("CreateApartmentPage")]
    public async Task<IActionResult> CreateApartmentPage([FromBody]CreateApartmentPageRequest request)
    {
        var validateResult = Validate(new CreateApartmentPageRequestValidator(), request);
        if (validateResult.Length!=0)
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
    
    [HttpPut("UpdateApartmentPage")]
    public async Task<IActionResult> UpdateApartmentPage([FromBody]UpdateApartmentPageRequest request)
    {
        var validateResult = Validate(new UpdateApartmentPageRequestValidator(), request);
        if (validateResult.Length!=0)
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