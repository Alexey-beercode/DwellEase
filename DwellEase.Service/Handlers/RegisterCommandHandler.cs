using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Identity;
using DwellEase.Service.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DwellEase.Service.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterRequest>
    {
        private readonly UserManager<User> _userManager;

        public RegisterCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterRequest> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                // Обработка ошибок регистрации, например, добавление их в ValidationResult.
                // Вам также можно выбрасывать исключение, если это необходимо.
            }

            var findUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (findUser == null)
            {
                throw new Exception($"User {request.Email} not found");
            }
            
            await _userManager.AddToRoleAsync(findUser,request.Role);

            return new RegisterRequest
            {
                Email = user.Email,
                UserName = user.UserName
            };
        }
    }
}