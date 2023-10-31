using System.Net;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Services.Implementations;
using DwellEase.Service.Services.Interfaces;
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
    private readonly IMediator _mediator;
    private readonly IImageService _imageService;

    public ApartmentPageController(ILogger<ApartmentPageController> logger, ApartmentPageService apartmentPageService, IMediator mediator, IImageService imageService)
    {
        _logger = logger;
        _apartmentPageService = apartmentPageService;
        _mediator = mediator;
        _imageService = imageService;
    }
    
    [Authorize(Policy = "CreatorArea")]
    [HttpPost("CreateApartmentPage")]
    public async Task<IActionResult> CreateApartmentPage([FromBody]CreateApartmentPageRequest request)
    {
        var response = _imageService.UploadImage(request.Images);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            return BadRequest();
        }
    }
}