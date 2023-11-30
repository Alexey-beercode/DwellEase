using DwellEase.DataManagement.Repositories.Interfaces;
using DwellEase.Domain.Entity;
using MongoDB.Driver;

namespace DwellEase.DataManagement.Repositories.Implementations;

public class SwitchPriorityRequestRepository:IBaseRepository<SwitchPriorityRequest>
{
    private readonly IMongoCollection<SwitchPriorityRequest> _collection;

    public SwitchPriorityRequestRepository(IMongoDatabase dataBase)
    {
        _collection = dataBase.GetCollection<SwitchPriorityRequest>("SwitchPriorityRequests");
    }
    public Task Create(SwitchPriorityRequest model)
    {
        return _collection.InsertOneAsync(model);
    }

    public Task Delete(Guid id)
    {
        var filter = Builders<SwitchPriorityRequest>.Filter.Eq(a => a.Id, id);
        return _collection.DeleteOneAsync(filter);
    }

    public Task Update(SwitchPriorityRequest model)
    {
        var filter = Builders<SwitchPriorityRequest>.Filter.Eq(a => a.Id, model.Id);
        var update = Builders<SwitchPriorityRequest>.Update
            .Set(a => a.ApartmentPageId, model.ApartmentPageId)
            .Set(a => a.UserId, model.UserId)
            .Set(a => a.NewType, model.NewType)
            .Set(a => a.IsApproved, model.IsApproved);

        return _collection.FindOneAndUpdateAsync(filter, update);
    }

    public async Task<Task<List<SwitchPriorityRequest>>> GetAll()
    {
        var filter = Builders<SwitchPriorityRequest>.Filter.Empty;
        var users = await _collection.FindAsync(filter);
        return users.ToListAsync();
    }

    public async Task<Task<SwitchPriorityRequest>> GetById(Guid id)
    {
        var filter = Builders<SwitchPriorityRequest>.Filter.Eq(a => a.Id, id);
        return (await _collection.FindAsync(filter)).FirstOrDefaultAsync();
    }
}