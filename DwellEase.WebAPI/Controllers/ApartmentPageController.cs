using System.Net;
using DwellEase.Domain.Models;
using DwellEase.Service.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Controllers
{
    [Route("ApartmentPage")]
    public class ApartmentPageController : Controller
    {
        private readonly ApartmentPageService _apartmentPageService;
        private readonly ILogger<ApartmentPageController> _logger;

        public ApartmentPageController(ApartmentPageService apartmentPageService,
            ILogger<ApartmentPageController> logger)
        {
            _apartmentPageService = apartmentPageService;
            _logger = logger;
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
        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAllApartmentPages()
        {
            var response = await _apartmentPageService.GetApartmentPagesAsync();
            return HandleResponse(response);
        }

        [HttpGet("Get-ById/{id}")]
        public async Task<IActionResult> GetApartmentPageById(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format");
            }

            var response = await _apartmentPageService.GetApartmentPageByIdAsync(guid);
            return HandleResponse(response);
        }

    }
}