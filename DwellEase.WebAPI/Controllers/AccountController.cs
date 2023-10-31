using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
    private readonly TokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountsController> _logger;
    private readonly IMediator _mediator;

    public AccountsController(IConfiguration configuration, IMediator mediator, ILogger<AccountsController> logger,
        UserService userService, TokenService tokenService)
    {
        _configuration = configuration;
        _mediator = mediator;
        _logger = logger;
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpGet("GetUser")]
    public async Task<IActionResult> TestGetUserByToken()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        string userId = jsonToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        string userName = jsonToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
        string userEmail = jsonToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
        string role = jsonToken.Claims.First(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
        return Ok($"{userId}\n{userName}\n{userEmail}\n{role}");
    }


    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate(AuthRequest request)
    {
        var query = new LoginQuery
        {
            UserName = request.UserName,
            Password = request.Password
        };

        var loginResponse = await _mediator.Send(query);

        _logger.LogInformation($"{loginResponse.Token}");
        var authResponse = new AuthResponse()
        {
            Username = loginResponse.Username,
            Token = loginResponse.Token,
            Email = loginResponse.Email
        };
        return Ok(loginResponse);
    }


    [HttpPost("Register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
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
        if (response.StatusCode == HttpStatusCode.OK)
        {
            return BadRequest("User with this username exists");
        }

        if (request.Role.ToUpper() != "RESIDENT" && request.Role.ToUpper() != "CREATOR")
        {
            return BadRequest("Incorrect role");
        }

        try
        {
            var result = await _mediator.Send(new RegisterCommand
            {
                Role = request.Role,
                Email = request.Email,
                Password = request.Password,
                UserName = request.UserName,
                PhoneNumber = new PhoneNumber(request.PhoneNumber)
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

    [HttpPost("Refresh-Token")]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid client request");
        }

        var principal = _configuration.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
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

        if (response.Data.RefreshToken != tokenModel.RefreshToken ||
            response.Data.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());

        response.Data.RefreshTokenExpiryTime =
            DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenExpirationDays").Get<int>());
        await _userService.UpdateAsync(response.Data);

        return new ObjectResult(new
        {
            accessToken = newAccessToken,
        });
    }

    [Authorize]
    [HttpPost("Revoke/{username}")]
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
    [HttpPost("Revoke-All")]
    public async Task<IActionResult> RevokeAll()
    {
        var response = await _userService.GetAllAsync();
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