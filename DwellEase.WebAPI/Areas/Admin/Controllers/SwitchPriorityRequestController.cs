using DwellEase.Service.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Admin.Controllers;

[ApiController]
[Area("Admin")]
[Route("{area}/SwitchPriorityRequest")]
[Authorize(Policy = "AdminArea")]
public class SwitchPriorityRequestController:ControllerBase
{
    private readonly IMediator _mediator;
  

    public SwitchPriorityRequestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("ApprovePriorityRequest")]
    public async Task<IActionResult> ApprovePriorityRequest([FromBody] string id)
    {
       // короче отправляем асинк запрос на url апппрувприоритиреквест и дальше получаем ответ
       //все
       //  написал
       // я сеньер по шарапам
       try
       {
           await _mediator.Send(new ApprovePriorityRequestCommand() { Id = id });
           return Ok();
       }
       catch (Exception e)
       {
           return BadRequest(e.Message);
       }
    }
}