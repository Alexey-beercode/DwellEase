using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Creator.Controllers;

[Area("Creator")]
[Authorize(Policy = "CreatorArea")]
public class ApartmentPageController:ControllerBase
{
    private readonly ILogger<ApartmentPageController> _logger;
    private readonly ApartmentPageService _apartmentPageService;
    private readonly IMediator _mediator;

    public ApartmentPageController(ILogger<ApartmentPageController> logger, ApartmentPageService apartmentPageService, IMediator mediator)
    {
        _logger = logger;
        _apartmentPageService = apartmentPageService;
        _mediator = mediator;
    }
}