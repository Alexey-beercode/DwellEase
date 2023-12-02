using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Service.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DwellEase.WebAPI.Controllers
{
    [ApiController]
    [Route("ApartmentPage")]
    public class ApartmentPageController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApartmentPageController> _logger;

        public ApartmentPageController(IMediator mediator, ILogger<ApartmentPageController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [SwaggerOperation("Gets a list of apartment pages")]
        [HttpGet("Get-All")]
        [SwaggerResponse(statusCode: 400, description: "Invalid request")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<ApartmentPage>))]
        public async Task<IActionResult> GetAllApartmentPages()
        {
            BaseResponse<List<ApartmentPage>> response = await _mediator.Send(new GetAllApartmentPagesQuery());
            return HandleResponse(response);
        }
        
        [SwaggerOperation("Gets a apartment page by id")]
        [SwaggerResponse(statusCode: 400, description: "Invalid request")]
        [SwaggerResponse(statusCode: 200, type: typeof(ApartmentPage))]
        [HttpGet("Get-ById/{id}")]
        public async Task<IActionResult> GetApartmentPageById(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format");
            }

            try
            {
                var page = await _mediator.Send(new GetApartmentPageByIdQuery(guid));
                return Ok(page);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
    }
}