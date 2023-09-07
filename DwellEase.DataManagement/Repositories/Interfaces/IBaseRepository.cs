namespace DwellEase.DataManagement.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    Task Create(T model);
    Task Delete(int id);
    Task Update(T model);
    Task<Task<List<T>>> GetAll();
    Task<Task<T>> GetById(int id);
}