﻿using System.Net;
using System.Text;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
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

    public ApartmentPageController(ILogger<ApartmentPageController> logger, ApartmentPageService apartmentPageService, IMediator mediator, IImageService imageService)
    {
        _logger = logger;
        _apartmentPageService = apartmentPageService;
        _imageService = imageService;
    }
    
    [Authorize(Policy = "CreatorArea")]
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
            return BadRequest(imageServiceResponse.Description);
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
                Title = request.Title
            },
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
}