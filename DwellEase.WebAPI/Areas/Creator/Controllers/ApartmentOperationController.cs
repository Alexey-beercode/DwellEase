using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Creator.Controllers;

[Area("Creator")]
[Authorize(Policy = "CreatorArea")]
public class ApartmentOperationController:ControllerBase
{
    private readonly ILogger<ApartmentOperationController> _logger;
    private readonly ApartmentOperationService _apartmentOperationService;
    private readonly IMediator _mediator;
    private readonly ApartmentPageService _apartmentPageService;

    public ApartmentOperationController(ILogger<ApartmentOperationController> logger, ApartmentOperationService apartmentOperationService, IMediator mediator, ApartmentPageService apartmentPageService)
    {
        _logger = logger;
        _apartmentOperationService = apartmentOperationService;
        _mediator = mediator;
        _apartmentPageService = apartmentPageService;
    }
    
}