using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Commands;
using DwellEase.Service.Queries.Creator;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DwellEase.WebAPI.Controllers;

[ApiController]
[Route("ApartmentOperation")]
public class ApartmentOperationController : ControllerBase
{
    private readonly ILogger<ApartmentOperationController> _logger;
    private readonly IMediator _mediator;
    private readonly ApartmentPageService _apartmentPageService;
    private readonly UserService _userService;
    private readonly StringToGuidMapper _guidMapper;

    public ApartmentOperationController(ILogger<ApartmentOperationController> logger, IMediator mediator,
        ApartmentPageService apartmentPageService, UserService userService, StringToGuidMapper guidMapper)
    {
        _logger = logger;
        _mediator = mediator;
        _apartmentPageService = apartmentPageService;
        _userService = userService;
        _guidMapper = guidMapper;
    }

    private IActionResult HandleResponse<T>(BaseResponse<T> response)
    {
        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogError($"Response from service status is not OK: {response.StatusCode}");
            return StatusCode((int)response.StatusCode, response.Description);
        }

        return Ok(response.Data);
    }

    [SwaggerOperation("Create rent operation")]
    [SwaggerResponse(statusCode: 200)]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [HttpPost("CreateRentOperation")]
    public async Task<IActionResult> CreateRentOperation([FromBody] RentRequest request)
    {
        var apartmentPageId = _guidMapper.MapTo(request.ApartmentPageId);
        var userId = _guidMapper.MapTo(request.UserId);

        var apartmentPageResponse = await _apartmentPageService.GetByIdAsync(apartmentPageId);
        var userResponse = await _userService.GetByIdAsync(userId);

        if (apartmentPageResponse.StatusCode != HttpStatusCode.OK)
        {
            return HandleResponse(apartmentPageResponse);
        }

        if (userResponse.StatusCode != HttpStatusCode.OK)
        {
            return HandleResponse(userResponse);
        }

        try
        {
            var result = await _mediator.Send(new CreateRentOperationCommand()
            {
                UserId = userId,
                ApartmentPageId = apartmentPageId,
                Price = apartmentPageResponse.Data.Price,
                OperationType = OperationType.Purchase,
                RentalPeriod = TimeSpan.Parse(request.RentalPeriod)
            });
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [SwaggerOperation("Create purchase operation")]
    [SwaggerResponse(statusCode: 200)]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [HttpPost("CreatePurchaseOperation")]
    public async Task<IActionResult> CreatePurchaseOperation([FromBody] PurchaseRequest request)
    {
        var apartmentPageId = _guidMapper.MapTo(request.ApartmentPageId);
        var userId = _guidMapper.MapTo(request.UserId);

        var apartmentPageResponse = await _apartmentPageService.GetByIdAsync(apartmentPageId);
        var userResponse = await _userService.GetByIdAsync(userId);

        if (apartmentPageResponse.StatusCode != HttpStatusCode.OK)
        {
            return HandleResponse(apartmentPageResponse);
        }

        if (userResponse.StatusCode != HttpStatusCode.OK)
        {
            return HandleResponse(userResponse);
        }

        try
        {
            await _mediator.Send(new CreatePurchaseOperationCommand()
            {
                UserId = userId,
                ApartmentPageId = apartmentPageId,
                Price = apartmentPageResponse.Data.Price,
                OperationType = OperationType.Purchase
            });
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [SwaggerOperation("Gets a list of apartment operations")]
    [SwaggerResponse(statusCode: 400, description: "Invalid request")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<ApartmentOperation>))]
    [HttpGet("GetAllOperations/{id}")]
    public async Task<IActionResult> GetAllApartmentPageOperations(string id)
    {
        try
        {
            var operations = await _mediator.Send(new GetAllApartmentOperationsQuery() { Id = id });
            return Ok(operations);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}