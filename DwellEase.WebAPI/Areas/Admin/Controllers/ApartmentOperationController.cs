using System.Runtime.CompilerServices;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "AdminArea")]
public class ApartmentOperationController:ControllerBase
{
    private readonly ILogger<ApartmentOperationController> _logger;
    private readonly ApartmentOperationService _apartmentOperationService;
    private readonly IMediator _mediator;

    public ApartmentOperationController(ILogger<ApartmentOperationController> logger, ApartmentOperationService apartmentOperationService, IMediator mediator)
    {
        _logger = logger;
        _apartmentOperationService = apartmentOperationService;
        _mediator = mediator;
    }
    
}