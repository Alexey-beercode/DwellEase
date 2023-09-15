using System.Net;
using DwellEase.Service.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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

        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAllApartmentPages()
        {
            var response = await _apartmentPageService.GetApartmentPagesAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Response from service status is not OK");
                return new JsonResult(response.Description);
            }

            _logger.LogInformation("Successfully return all apartmentpages");
            return new JsonResult(response.Data[0].OwnerId);
        }
    }
}