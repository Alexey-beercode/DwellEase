namespace DwellEase.DataManagement.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    Task Create(T model);
    Task Delete(Guid id);
    Task Update(T model);
    Task<Task<List<T>>> GetAll();
    Task<Task<T>> GetById(Guid id);
}