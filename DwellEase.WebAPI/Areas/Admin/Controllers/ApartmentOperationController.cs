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
[Route("{area}/ApartmentOperation")]
public class ApartmentOperationController:ControllerBase
{
    private readonly ILogger<ApartmentOperationController> _logger;
    private readonly ApartmentOperationService _apartmentOperationService;

    public ApartmentOperationController(ILogger<ApartmentOperationController> logger, ApartmentOperationService apartmentOperationService)
    {
        _logger = logger;
        _apartmentOperationService = apartmentOperationService;
    }

    [SwaggerOperation("Gets a list of apartment operations")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<ApartmentOperation>))]
    [HttpGet("GetAllOperations")]
    public async Task<IActionResult> GetAllApartmentOperations()
    {
        var response = await _apartmentOperationService.GetAllAsync();
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            _logger.LogError(response.Description);
            return BadRequest(response.Description);
        }
        _logger.LogInformation("Successfuly return all operations");
        return Ok(response.Data);
    }
}