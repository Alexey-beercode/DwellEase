using DwellEase.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace DwellEase.DataManagement.DataSenders;

public class RoleDataSender
{
    public static void SeedData()
    {
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var mongoDatabase = mongoClient.GetDatabase("DwellEaseApartmentsDB");
        var roleCollection = mongoDatabase.GetCollection<Role>("Roles");
        if (!roleCollection.Find(_ => true).Any())
        {
            var role = new Role()
            {
                RoleName = "Admin",
                NormalizedRoleName = "ADMIN",
                Id = Guid.NewGuid()
            };

            roleCollection.InsertOneAsync(role);
        }
    }
}