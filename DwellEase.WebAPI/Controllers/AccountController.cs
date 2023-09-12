using System.IdentityModel.Tokens.Jwt;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Identity;
using DwellEase.Service.Commands;
using DwellEase.Service.Extensions;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Controllers;

[ApiController]
[Route("Account")]
public class AccountsController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;
    private readonly ApartmentPageService _apartmentPageService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountsController(UserManager<User> userManager,
        IConfiguration configuration, IMediator mediator, RoleManager<Role> roleManager, ApartmentPageService apartmentPageService, IHttpContextAccessor httpContext, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _configuration = configuration;
        _mediator = mediator;
        _apartmentPageService = apartmentPageService;
        _httpContextAccessor = httpContextAccessor;
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
        var user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
        var newRefreshToken = _configuration.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

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
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return BadRequest("Invalid user name");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return Ok();
    }

    [Authorize]
    [HttpPost]
    [Route("Revoke-all")]
    public async Task<IActionResult> RevokeAll()
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        return Ok();
    }
}