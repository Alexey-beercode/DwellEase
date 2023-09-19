using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Identity;
using DwellEase.Service.Extensions;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using DwellEase.Service.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DwellEase.Service.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponse>
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly RoleService _roleService;

        public LoginQueryHandler(IConfiguration configuration, ITokenService tokenService, UserService userService, RoleService roleService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<AuthResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var response = await _userService.FindByEmailAsync(request.Email);

            if (response.StatusCode!=HttpStatusCode.OK)
            {
               
            }

            var isPasswordValid = (await _userService.CheckPasswordAsync(response.Data, request.Password)).Data;

            if (!isPasswordValid)
            {
                throw new ApplicationException("Bad credentials");
            }

            var user =(await _userService.FindByEmailAsync(request.Email)).Data;

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var allRoles = (await _roleService.GetAllAsync()).Data;
            List<Guid> roleIds =allRoles.Where(r => r.RoleName == user.Role.RoleName).Select(x => x.Id).ToList();
            var roles = (await _roleService.GetAllAsync()).Data.Where(x => roleIds.Contains(x.Id)).ToList();

            var accessToken = _tokenService.CreateToken(user, roles);
            user.RefreshToken = _configuration.GenerateRefreshToken();
            user.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

            return new AuthResponse
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = accessToken,
                RefreshToken = user.RefreshToken
            };
        }
    }
}
