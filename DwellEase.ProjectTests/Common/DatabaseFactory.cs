using MongoDB.Driver;
using Moq;

namespace DwellEase.ProjectTests.Common
{
    public static class DatabaseFactory
    {   
        public static IMongoDatabase Create()
        {
            var mongoClient = new Mock<MongoClient>();
            var database = mongoClient.Object.GetDatabase("DwellEaseTest");
            database.CreateCollectionAsync("Users");
            database.CreateCollectionAsync("Roles");
            database.CreateCollectionAsync("ApartmentPages");
            database.CreateCollectionAsync("ApartmentOperations");
            database.CreateCollectionAsync("UserRole");
            database.CreateCollectionAsync("SwitchPriorityRequests");
            return database;
        }
    }
}
