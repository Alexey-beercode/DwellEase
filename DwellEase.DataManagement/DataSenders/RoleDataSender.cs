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
                Id = new Guid("9b0dc8cd-35a0-4ca6-b820-74052c74417b")
            };

            roleCollection.InsertOneAsync(role);
        }
    }
}