using DwellEase.Domain.Entity;

namespace DwellEase.DataManagement.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    Task Create(T model);
    Task Delete(int id);
    Task Update(T model);
    Task<Task<List<ApartmentPage>>> GetAll();
    Task<Task<ApartmentPage>> GetById(int id);
}