using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Responses;

namespace DwellEase.Service.Services.Interfaces;

public interface IService<T>
{
    Task<BaseResponse<T>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<T>>> GetAllAsync();
}