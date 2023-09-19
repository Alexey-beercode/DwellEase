﻿using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Controllers
{
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

        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAllApartmentPages()
        {
            BaseResponse<List<ApartmentPage>> response = await _mediator.Send(new GetAllApartmentPagesQuery());
            return HandleResponse(response);
        }

        [HttpGet("Get-ById/{id}")]
        public async Task<IActionResult> GetApartmentPageById(string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid ID format");
            }

            BaseResponse<ApartmentPage> response = await _mediator.Send(new GetApartmentPageByIdQuery(guid));
            return HandleResponse(response);
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