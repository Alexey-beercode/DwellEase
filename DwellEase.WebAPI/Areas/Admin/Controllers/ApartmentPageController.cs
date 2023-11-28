using System.Net;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Admin.Controllers;

[ApiController]
[Area("Admin")]
[Authorize(Policy = "AdminArea")]
[Route("{area}/ApartmentPage")]
public class ApartmentPageController:ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ApartmentPageController> _logger;
    private readonly ApartmentPageService _apartmentPageService;

    public ApartmentPageController(IMediator mediator, ILogger<ApartmentPageController> logger, ApartmentPageService apartmentPageService)
    {
        _mediator = mediator;
        _logger = logger;
        _apartmentPageService = apartmentPageService;
    }

    [HttpGet("GetAllPages")]
    public async Task<IActionResult> GetAllPages()
    {
        var response = await _apartmentPageService.GetAllAsync();
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            return BadRequest(response.Description);
        }

        return Ok(response.Data);
    }

    [HttpDelete("DeletePage")]
    public async Task<IActionResult> DeleteApartmentPage([FromBody] string id)
    {
        if (Guid.TryParse(id,out var guid))
        {
            return BadRequest("Invalid id");
        }
        
        var response = await _apartmentPageService.DeleteAsync(guid);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            return BadRequest(response.Description);
        }

        return Ok();
    }
}