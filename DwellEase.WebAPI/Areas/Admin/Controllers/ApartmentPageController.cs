using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Admin.Controllers;

[ApiController]
[Area("Admin")]
[Authorize(Policy = "AdminArea")]
[Route("{area}/ApartmentPage")]
public class ApartmentPageController:ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ApartmentPageController> _logger;

    public ApartmentPageController(IMediator mediator, ILogger<ApartmentPageController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
}