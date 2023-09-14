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
            var admin = new Role()
            {
                RoleName = "Admin",
                NormalizedRoleName = "ADMIN",
                Id = new Guid("9b0dc8cd-35a0-4ca6-b820-74052c74417b")
            };
            var user = new Role()
            {
                RoleName = "Resident",
                NormalizedRoleName = "RESIDENT",
                Id=new Guid("3b0b2b43-b58a-4cf7-a489-45abd3789cb7")
            };
            var creator = new Role()
            {
                RoleName = "Creator",
                NormalizedRoleName = "CREATOR",
                Id= new Guid("cfcfe2d0-7c45-43d3-8df4-02dcb715ed40")
            };
            Role[] roles = new []{ user, admin, creator };

            roleCollection.InsertManyAsync(roles);
        }
    }
}