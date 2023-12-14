using MongoDB.Driver;
using Moq;

namespace DwellEase.ProjectTests.Common.Interfaces;

public interface IDataInitializer<T>
{
    void Initial(ref IMongoCollection<T> collection);
}