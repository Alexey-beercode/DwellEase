using DwellEase.DataManagement.Repositories.Interfaces;
using DwellEase.Domain.Entity;
using MongoDB.Driver;

namespace DwellEase.DataManagement.Repositories.Implementations;

public class ApartmentPageRepository : IBaseRepository<ApartmentPage>
{
    private readonly IMongoCollection<ApartmentPage> _collection;

    public ApartmentPageRepository(IMongoDatabase dataBase)
    {
        _collection = dataBase.GetCollection<ApartmentPage>("ApartmentPages");
    }


    public Task Create(ApartmentPage model)
    {
        return _collection.InsertOneAsync(model);
    }

    public Task Delete(int id)
    {
        var filter = Builders<ApartmentPage>.Filter.Eq(a => a.Id, id);
        return _collection.DeleteOneAsync(filter);
    }

    public Task Update(ApartmentPage model)
    {
        var filter = Builders<ApartmentPage>.Filter.Eq(a => a.Id, model.Id);
        var update = Builders<ApartmentPage>.Update
            .Set(a => a.DaylyPrice, model.DaylyPrice)
            .Set(a => a.Price, model.Price)
            .Set(a => a.IsAvailableForPurchase, model.IsAvailableForPurchase)
            .Set(a => a.Status, model.Status)
            .Set(a => a.Apartment, model.Apartment)
            .Set(a => a.PhoneNumber, model.PhoneNumber)
            .Set(a => a.OwnerId, model.OwnerId)
            .Set(a => a.Images, model.Images)
            .Set(a=>a.Date,model.Date)
            .Set(a=>a.ApprovalStatus,model.ApprovalStatus)
            .Set(a=>a.PriorityType,model.PriorityType);

        return _collection.FindOneAndUpdateAsync(filter, update);
    }

    public async Task<Task<List<ApartmentPage>>> GetAll()
    {
        var filter = Builders<ApartmentPage>.Filter.Empty;
        var apartmentPages = await _collection.FindAsync(filter);
        return apartmentPages.ToListAsync();
    }

    public async Task<Task<ApartmentPage>> GetById(int id)
    {
        var filter = Builders<ApartmentPage>.Filter.Eq(a => a.Id, id);
        return (await _collection.FindAsync(filter)).FirstOrDefaultAsync();
    }
}