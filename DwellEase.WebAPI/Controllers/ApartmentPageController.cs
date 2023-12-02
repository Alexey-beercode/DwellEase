using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Responses;
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

        public ApartmentPageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [SwaggerOperation("Gets a list of apartment pages")]
        [HttpGet("Get-All")]
        [SwaggerResponse(statusCode: 400, description: "Invalid request")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<ApartmentPageResponse>))]
        public async Task<IActionResult> GetAllApartmentPages()
        {
            try
            {
                var apartmentPageResponses =await _mediator.Send(new GetAllApartmentPagesQuery());
                return Ok(apartmentPageResponses);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [SwaggerOperation("Gets a apartment page by id")]
        [SwaggerResponse(statusCode: 400, description: "Invalid request")]
        [SwaggerResponse(statusCode: 200, type: typeof(ApartmentPageResponse))]
        [HttpGet("Get-ById/{id}")]
        public async Task<IActionResult> GetApartmentPageById(string id)
        {

            try
            {
                var page = await _mediator.Send(new GetApartmentPageByIdQuery(){Id = id});
                return Ok(page);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}