using System.Net;
using System.Text;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Queries.Creator;
using DwellEase.Service.Services.Implementations;
using DwellEase.Service.Services.Interfaces;
using DwellEase.WebAPI.Validators;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Creator.Controllers;

[Area("Creator")]
[Route("ApartmentPage")]
public class ApartmentPageController:ControllerBase
{
    private readonly ILogger<ApartmentPageController> _logger;
    private readonly ApartmentPageService _apartmentPageService;
    private readonly IImageService _imageService;
    private readonly IMediator _mediator;

    public ApartmentPageController(ILogger<ApartmentPageController> logger, ApartmentPageService apartmentPageService, IMediator mediator, IImageService imageService)
    {
        _logger = logger;
        _apartmentPageService = apartmentPageService;
        _mediator = mediator;
        _imageService = imageService;
    }
    
    [HttpPost("CreateApartmentPage")]
    public async Task<IActionResult> CreateApartmentPage([FromBody]CreateApartmentPageRequest request)
    {
        CreateApartmentPageRequestValidator validator = new CreateApartmentPageRequestValidator();
        var validateResult = validator.ValidateAsync(request).Result;
        if (!validateResult.IsValid)
        {
            var errors =new StringBuilder();
            validateResult.Errors.ForEach(a => errors.Append(a.ErrorMessage).Append("\n"));
            return BadRequest(errors);
        }
        var imageServiceResponse = _imageService.UploadImage(request.Images);
        if (imageServiceResponse.StatusCode!=HttpStatusCode.OK)
        {
            return StatusCode((int)imageServiceResponse.StatusCode, imageServiceResponse.Description);
        }

        if (!Guid.TryParse(request.OwnerId,out Guid ownerGuidId))
        {
            return BadRequest("OwnerId is not valid");
        }
        if (!ApartmentType.TryParse(request.ApartmentType, out ApartmentType apartmentType))
        {
            return BadRequest("Apartment type is not valid");
        }

        ApartmentPage apartmentPage = new ApartmentPage()
        {
            Apartment = new Apartment()
            {
                Address = new Address()
                {
                    Building = request.Building,
                    City = request.City,
                    HouseNumber = request.HouseNumber,
                    Street = request.Street
                },
                ApartmentType = apartmentType,
                Area = request.Area,
                Rooms = request.Rooms,
                
            },
            Title = request.Title,
            DaylyPrice = request.DailyPrice,
            Price = request.Price,
            Images = imageServiceResponse.Data,
            IsAvailableForPurchase = request.IsAvailableForPurchase,
            OwnerId = ownerGuidId,
            PhoneNumber = new PhoneNumber(request.PhoneNumber)
        };
        await _apartmentPageService.CreateAsync(apartmentPage);
        _logger.LogInformation("Successfully create Apartment page");
        return Ok();
    }
    
    [HttpPut("UpdateApartmentPage")]
    public async Task<IActionResult> UpdateApartmentPage([FromBody]UpdateApartmentPageRequest request)
    {
        UpdateApartmentPageRequestValidator validator = new UpdateApartmentPageRequestValidator();
        var validateResult = validator.ValidateAsync(request).Result;
        if (!validateResult.IsValid)
        {
            var errors =new StringBuilder();
            validateResult.Errors.ForEach(a => errors.Append(a.ErrorMessage).Append("\n"));
            return BadRequest(errors);
        }
        if (!Guid.TryParse(request.PageId,out Guid guidId))
        {
            return BadRequest("OwnerId is not valid");
        }

        var response = await _apartmentPageService.GetByIdAsync(guidId);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response.Description);
        }

        var newApartmentPage = new ApartmentPage()
        {
            Apartment = response.Data.Apartment,
            ApprovalStatus = response.Data.ApprovalStatus,
            Date = response.Data.Date,
            DaylyPrice = request.DailyPrice,
            Price = request.Price,
            Title = request.Title,
            Id = response.Data.Id,
            OwnerId = response.Data.OwnerId,
            Images = response.Data.Images,
            IsAvailableForPurchase = request.IsAvailableForPurchase,
            Status = response.Data.Status,
            PriorityType = response.Data.PriorityType,
            PhoneNumber = new PhoneNumber(request.PhoneNumber)
        };
        await _apartmentPageService.EditAsync(newApartmentPage);
        return Ok();
    }
    
    [HttpDelete("DeleteApartmentPage")]
    public async Task<IActionResult> DeleteApartmentPage([FromBody] string id)
    {
        if (!Guid.TryParse(id,out Guid guidId))
        {
            return BadRequest("OwnerId is not valid");
        }

        var response = await _apartmentPageService.GetByIdAsync(guidId);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response.Description);
        }
        await _apartmentPageService.DeleteAsync(guidId);
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