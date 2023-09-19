using System.Net;
using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Services.Implementations;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly RoleService _roleService;

    public UserService(UserRepository userRepository, ILogger<UserService> logger, RoleService roleService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _roleService = roleService;
    }

    private BaseResponse<T> HandleError<T>(string description, HttpStatusCode error)
    {
        var response = new BaseResponse<T>
        {
            StatusCode = error,
            Description = description
        };
        _logger.LogError(description);
        return response;
    }

    public async Task<BaseResponse<bool>> CreateAsync(User user, string password)
    {
        var workFactor = 12;
        var salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor);
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
        user.PasswordSalt = salt;
        user.PasswordHash = hashedPassword;
        await _userRepository.Create(user);
        return new BaseResponse<bool>() { StatusCode = HttpStatusCode.OK };
    }


    public async Task<BaseResponse<bool>> AddToRoleAsync(User user, string role)
    {
        if (!(role == "Admin" || role == "Creator" || role == "Resident"))
        {
            HandleError<bool>("Role are not contains right role type", HttpStatusCode.BadRequest);
        }

        var roles = (await _roleService.GetAllAsync()).Data;
        user.Role = roles.FirstOrDefault(a => a.RoleName == role);
        await _userRepository.Update(user);
        return new BaseResponse<bool>() { StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
    {
        var user = await await _userRepository.GetById(id);
        if (user == null)
        {
            return HandleError<bool>($"User with id: {id} not found", HttpStatusCode.NoContent);
        }

        await _userRepository.Delete(id);
        _logger.LogInformation($"Successfully delete user with id: {id}");
        return new BaseResponse<bool>() { StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<User>> GetByIdAsync(Guid id)
    {
        var user = await await _userRepository.GetById(id);
        if (user == null)
        {
            return HandleError<User>($"User with id: {id} not found", HttpStatusCode.NoContent);
        }

        _logger.LogInformation($"Successfully get user with id: {id}");
        return new BaseResponse<User>() { Data = user, StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<User>> FindByNameAsync(string userName)
    {
        var user = (await await _userRepository.GetAll()).FirstOrDefault(a => a.UserName == userName);
        if (user == null)
        {
            return HandleError<User>($"User with username: {userName} not found", HttpStatusCode.NoContent);
        }

        return new BaseResponse<User>() { Data = user, StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<bool>> UpdateAsync(User user)
    {
        var findUser = await await _userRepository.GetById(user.Id);
        if (findUser == null)
        {
            return HandleError<bool>($"User with id: {user.Id} not found", HttpStatusCode.NoContent);
        }

        await _userRepository.Update(user);
        return new BaseResponse<bool>() { StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<User>> FindByEmailAsync(string email)
    {
        var user = (await await _userRepository.GetAll()).FirstOrDefault(a => a.Email == email);
        if (user == null)
        {
            return HandleError<User>($"User with email: {email} not found", HttpStatusCode.NoContent);
        }

        return new BaseResponse<User>() { Data = user, StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<bool>> CheckPasswordAsync(User user, string password)
    {
        var findUser =await await _userRepository.GetById(user.Id);
        if (findUser == null)
        {
            return HandleError<bool>($"User with id: {user.Id} not found", HttpStatusCode.NoContent);
        }

        // Используйте соль из базы данных при проверке пароля
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, findUser.PasswordHash);

        return new BaseResponse<bool>()
        {
            Data = isPasswordValid,
            StatusCode = HttpStatusCode.OK
        };
    }



    public async Task<BaseResponse<List<User>>> GetAllAsync()
    {
        var users = await await _userRepository.GetAll();
        if (users.Count == 0)
        {
            return HandleError<List<User>>("Users are not found", HttpStatusCode.NoContent);
        }

        return new BaseResponse<List<User>>() { Data = users, StatusCode = HttpStatusCode.OK };
    }
}