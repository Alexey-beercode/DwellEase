using System.IdentityModel.Tokens.Jwt;
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
    private readonly TokenService _tokenService;

    public AccountsController(IConfiguration configuration, IMediator mediator, ILogger<AccountsController> logger, ApartmentPageService apartmentPageService, UserService userService,TokenService tokenService)
    {
        _configuration = configuration;
        _mediator = mediator;
        _logger = logger;
        _apartmentPageService = apartmentPageService;
        _userService = userService;
        _tokenService = tokenService;
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
        var user =await _userService.FindByNameAsync("Alexey");
        return Ok(user.Data.RefreshTokenExpiryTime);
    }
   
    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate(AuthRequest request)
    {
        var query = new LoginQuery
        {
            UserName = request.UserName,
            Password = request.Password
        };

        var authResponse = await _mediator.Send(query);

        return Ok(authResponse);
    }


    [HttpPost("Register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(request);
        }

        if (!PhoneNumber.IsPhoneValid(request.PhoneNumber))
        {
            return BadRequest("Phone number is not valid");
        }
        
        var response = await _userService.FindByNameAsync(request.UserName);
        if (response.StatusCode==HttpStatusCode.OK)
        {
            return Unauthorized("User with this username exists");
        }

        try
        {
            var result = await _mediator.Send(new RegisterCommand
            {
                Role = request.Role,
                Email = request.Email,
                Password = request.Password,
                UserName = request.UserName,
                PhoneNumber=new PhoneNumber(request.PhoneNumber)
            });
            return await Authenticate(new AuthRequest
            {
                UserName = result.UserName,
                Password = request.Password
            });
        }
        catch (Exception e)
        { 
            return BadRequest(e.Message);
        }
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

    [Authorize(Policy = "AdminArea")]
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