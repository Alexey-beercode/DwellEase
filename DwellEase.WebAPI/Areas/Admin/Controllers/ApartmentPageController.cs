using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Service.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DwellEase.WebAPI.Areas.Admin.Controllers;

[ApiController]
[Area("Admin")]
[Authorize(Policy = "AdminArea")]
[Route("{area}/ApartmentPage")]
public class ApartmentPageController:ControllerBase
{
    private readonly ApartmentPageService _apartmentPageService;

    public ApartmentPageController(ApartmentPageService apartmentPageService)
    {
        _apartmentPageService = apartmentPageService;
    }

    [SwaggerOperation("Gets a list of apartment pages")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<ApartmentPage>))]
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

    [SwaggerOperation("Delete apartment page by id")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200)]
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