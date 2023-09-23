using System.Net;
using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.Domain.Models.Identity;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Linq;

namespace DwellEase.Service.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponse>
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;
        private readonly UserRoleRepository _userRoleRepository;
        private readonly RoleService _roleService;

        public LoginQueryHandler(IConfiguration configuration, TokenService tokenService, UserService userService, UserRoleRepository userRoleRepository, RoleService roleService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _userService = userService;
            _userRoleRepository = userRoleRepository;
            _roleService = roleService;
        }

        public async Task<LoginResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var response = await _userService.FindByNameAsync(request.UserName);

            if (response.StatusCode!=HttpStatusCode.OK)
            {
                throw new AggregateException("User are not found");
            }

            var isPasswordValid = (await _userService.CheckPasswordAsync(response.Data, request.Password)).Data;

            if (!isPasswordValid)
            {
                throw new ApplicationException("Bad credentials");
            }

            var user = response.Data;

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var roles =(await _roleService.GetAllAsync()).Data;
            var rolesId = (await _userRoleRepository.GetAll())
                .Where(a=>a.UserId==user.Id)
                .Select(a=>a.RoleId)
                .ToList();
            var userRoles = roles.Where(role => rolesId.Contains(role.Id)).ToList();
            var accessTokenClaims = _tokenService.CreateClaims(user,userRoles);
            var accessToken = _tokenService.GenerateAccessToken(accessTokenClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenExpirationDays").Get<int>());
            await _userService.UpdateAsync(user);
            
            return new LoginResponse
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
