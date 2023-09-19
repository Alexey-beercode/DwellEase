using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Identity;
using DwellEase.Service.Commands;
using DwellEase.Service.Services.Implementations;
using MediatR;

namespace DwellEase.Service.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthRequest>
    {
        private readonly UserService _userSrevice;

        public RegisterCommandHandler(UserService userSrevice)
        {
            _userSrevice = userSrevice;
        }

        public async Task<AuthRequest> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                NormalizedUserName = request.UserName.ToUpper(),
                PhoneNumber = request.PhoneNumber
            };
            
            await _userSrevice.CreateAsync(user, request.Password);
            

            var response = await _userSrevice.FindByNameAsync(request.UserName);
            if (response.StatusCode!=HttpStatusCode.OK)
            {
                throw new Exception($"User {request.Email} not found");
            }
            
            await _userSrevice.AddToRoleAsync(response.Data,request.Role);

            return new AuthRequest
            {
                Password = request.Password,
                UserName = user.UserName
            };
        }
    }
}