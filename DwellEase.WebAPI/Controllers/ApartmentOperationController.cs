using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Commands;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Controllers;

public class ApartmentOperationController:ControllerBase
{
    private readonly ApartmentOperationService _apartmentOperationService;
    private readonly ILogger<ApartmentOperationController> _logger;
    private readonly IMediator _mediator;
    private readonly ApartmentPageService _apartmentPageService;
    private readonly UserService _userService;

    public ApartmentOperationController(ApartmentOperationService apartmentOperationService, ILogger<ApartmentOperationController> logger, IMediator mediator, ApartmentPageService apartmentPageService, UserService userService)
    {
        _apartmentOperationService = apartmentOperationService;
        _logger = logger;
        _mediator = mediator;
        _apartmentPageService = apartmentPageService;
        _userService = userService;
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
    
    [HttpPut("CreateRentOperation")]
    public async Task<IActionResult> CreateRentOperation([FromBody] RentRequest request)
    {
        var isApartmentPageIdValid=Guid.TryParse(request.ApartmentPageId, out var apartmentPageId);
        var isUserIdValid=Guid.TryParse(request.UserId, out var userId);
        if (!isUserIdValid)
        {
            return BadRequest("Invalid user ID format");
        }
        if(!isApartmentPageIdValid)
        {
            return BadRequest("Invalid apartmentpage ID format");
        }

        var apartmentPageResponse = await _apartmentPageService.GetByIdAsync(apartmentPageId);
        var userResponse = await _userService.GetByIdAsync(userId);
        
        if (apartmentPageResponse.StatusCode!=HttpStatusCode.OK)
        {
            return HandleResponse(apartmentPageResponse);
        }

        if (userResponse.StatusCode!=HttpStatusCode.OK)
        {
            return HandleResponse(userResponse);
        }

        try
        {
            var result = await _mediator.Send(new CreatePurchaseOperationCommand()
            {
                UserId = userId,
                ApartmentPageId = apartmentPageId,
                Price = apartmentPageResponse.Data.Price,
                OperationType = OperationType.Purchase
            });
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("CreatePurchaseOperation")]
    public async Task<IActionResult> CreatePurchaseOperation([FromBody] PurchaseRequest request)
    {
        var isApartmentPageIdValid=Guid.TryParse(request.ApartmentPageId, out var apartmentPageId);
        var isUserIdValid=Guid.TryParse(request.UserId, out var userId);
        if (!isUserIdValid)
        {
            return BadRequest("Invalid user ID format");
        }
        if(!isApartmentPageIdValid)
        {
            return BadRequest("Invalid apartmentpage ID format");
        }

        var apartmentPageResponse = await _apartmentPageService.GetByIdAsync(apartmentPageId);
        var userResponse = await _userService.GetByIdAsync(userId);
        
        if (apartmentPageResponse.StatusCode!=HttpStatusCode.OK)
        {
            return HandleResponse(apartmentPageResponse);
        }

        if (userResponse.StatusCode!=HttpStatusCode.OK)
        {
            return HandleResponse(userResponse);
        }

        try
        {
            var result = await _mediator.Send(new CreatePurchaseOperationCommand()
            {
                UserId = userId,
                ApartmentPageId = apartmentPageId,
                Price = apartmentPageResponse.Data.Price,
                OperationType = OperationType.Purchase
            });
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}