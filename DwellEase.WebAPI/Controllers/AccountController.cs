﻿using System.IdentityModel.Tokens.Jwt;
using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Identity;
using DwellEase.Service.Commands;
using DwellEase.Service.Extensions;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Controllers;

[ApiController]
[Route("Account")]
public class AccountsController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ApartmentPageService _apartmentPageService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountsController> _logger;
    private readonly IMediator _mediator;
    private readonly ApartmentOperationService _apartmentOperationService;

    public AccountsController(IConfiguration configuration, IMediator mediator, ILogger<AccountsController> logger, ApartmentPageService apartmentPageService, ApartmentOperationService apartmentOperationService, UserService userService)
    {
        _configuration = configuration;
        _mediator = mediator;
        _logger = logger;
        _apartmentPageService = apartmentPageService;
        _apartmentOperationService = apartmentOperationService;
        _userService = userService;
    }
    
    [HttpGet("log")]
    public async Task<IActionResult> Log()
    {
        var apartmentPage = new ApartmentPage()
            {
                Apartment = new Apartment()
                {
                    Address = new Address()
                    {
                        Building = "A",
                        City = "Minsk",
                        HouseNumber = 48,
                        Street = "Yakubova"
                    },
                    ApartmentType = ApartmentType.Flat,
                    Area = 95,
                    Rooms = 3,
                    Title = "Квартира в Минске"
                },
                DaylyPrice = 300,
                IsAvailableForPurchase = true,
                PhoneNumber = new PhoneNumber("+375445983720"),
                Price = 90000
            };
        var operation = new ApartmentOperation()
        {
            ApartmentPageId = new Guid("da52082f-a3eb-4917-89bb-57a14bd41c98"),
            StartDate = DateTime.Now,
            EndDate = new DateTime(2023, 9, 19, 13, 17, 00).ToUniversalTime(),
            OperationType = OperationType.Rent,
            Price = apartmentPage.DaylyPrice
        };
        //await _apartmentPageService.EditApartmentPageAsync(apartmentPage);
        //await _apartmentPageService.CreateApartmentPageAsync(apartmentPage, new Guid());
        // await _apartmentOperationService.CreateRentOperationAsync(operation,new Guid());
        //var response = await _apartmentOperationService.GetApartmenOperationsAsync();
        var response =
            await _apartmentPageService.GetByIdAsync(new Guid("da52082f-a3eb-4917-89bb-57a14bd41c98"));
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            _logger.LogError("Status code from service is not Ok");
            return NotFound();
        }
        return Ok(response);
    }

    [HttpGet("lg")]
    public async Task<IActionResult> LogG()
    {
        var user = new User()
        {
            UserName = "Alexanderrr",
            Email = "cripster12@yandex.ru",
        };
        await _userService.CreateAsync(user, "CHEATS145");
        var response = await _userService.FindByNameAsync("Alexander1");
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response.Description);
        }
        await _userService.AddToRoleAsync(response.Data, "Admin");
        return Ok(response.Data.Role.RoleName);
    }
   
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        var query = new LoginQuery
        {
            Email = request.Email,
            Password = request.Password
        };

        var authResponse = await _mediator.Send(query);

        return Ok(authResponse);
    }


    [HttpPost("Register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(request);
        }

        var result = await _mediator.Send(new RegisterCommand
        {
            Email = request.Email,
            Password = request.Password,
            PasswordConfirm = request.PasswordConfirm,
            UserName = request.UserName
        });

        return await Authenticate(new AuthRequest
        {
            Email = result.Email,
            Password = result.Password
        });
    }

    [HttpPost]
    [Route("Refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel? tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid client request");
        }

        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;
        var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

        if (principal == null)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var username = principal.Identity!.Name;
        var response = await _userService.FindByNameAsync(username!);
        
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response.Description);
        }
        
        if (response.Data.RefreshToken != refreshToken || response.Data.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
        var newRefreshToken = _configuration.GenerateRefreshToken();

        response.Data.RefreshToken = newRefreshToken;
        await _userService.UpdateAsync(response.Data);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    [Authorize]
    [HttpPost]
    [Route("Revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var response = await _userService.FindByNameAsync(username);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response.Description);
        }

        response.Data.RefreshToken = null;
        await _userService.UpdateAsync(response.Data);

        return Ok();
    }

    [Authorize]
    [HttpPost]
    [Route("Revoke-all")]
    public async Task<IActionResult> RevokeAll()
    {
        var response =await _userService.GetAllAsync();
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode((int)response.StatusCode, response.Description);
        }

        var users = response.Data;
        foreach (var user in users)
        {
            user.RefreshToken = null;
            await _userService.UpdateAsync(user);
        }

        return Ok();
    }
}