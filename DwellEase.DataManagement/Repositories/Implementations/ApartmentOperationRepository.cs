using DwellEase.DataManagement.Repositories.Interfaces;
using DwellEase.Domain.Entity;
using MongoDB.Driver;

namespace DwellEase.DataManagement.Repositories.Implementations;

public class ApartmentOperationRepository:IBaseRepository<ApartmentOperation>
{
    
    private readonly IMongoCollection<ApartmentOperation> _collection;
    
    public ApartmentOperationRepository(IMongoDatabase dataBase)
    {
        _collection = dataBase.GetCollection<ApartmentOperation>("ApartmentOperations");
    }
    public Task Create(ApartmentOperation model)
    {
        return _collection.InsertOneAsync(model);
    }

    public Task Delete(Guid id)
    {
        var filter = Builders<ApartmentOperation>.Filter.Eq(a => a.Id.ToString(), id.ToString());
        return _collection.DeleteOneAsync(filter);
    }

    public Task Update(ApartmentOperation model)
    {
        var filter = Builders<ApartmentOperation>.Filter.Eq(a => a.Id, model.Id);
        var update = Builders<ApartmentOperation>.Update
            .Set(a => a.ApartmentPage, model.ApartmentPage)
            .Set(a => a.UserId, model.UserId)
            .Set(a => a.StartDate, model.StartDate)
            .Set(a => a.EndDate, model.EndDate)
            .Set(a => a.OperationType, model.OperationType)
            .Set(a => a.Price, model.Price);

        return _collection.FindOneAndUpdateAsync(filter, update);
    }

    public async Task<Task<List<ApartmentOperation>>> GetAll()
    {
        var filter = Builders<ApartmentOperation>.Filter.Empty;
        var apartmentOperations = await _collection.FindAsync(filter);
        return apartmentOperations.ToListAsync();
    }

    public async Task<Task<ApartmentOperation>> GetById(Guid id)
    {
        var filter = Builders<ApartmentOperation>.Filter.Eq(a => a.Id.ToString(), id.ToString());
        return (await _collection.FindAsync(filter)).FirstOrDefaultAsync();
    }
}