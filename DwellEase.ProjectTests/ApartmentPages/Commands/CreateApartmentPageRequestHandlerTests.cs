using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models.Requests;
using DwellEase.ProjectTests.Common;
using DwellEase.Service.Handlers.Creator;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

namespace DwellEase.ProjectTests.ApartmentPages.Commands;

public class CreateApartmentPageRequestHandlerTests
{
    private IMongoDatabase _database = DatabaseFactory.Create();
    private ApartmentPageService _apartmentPageService;
    private ApartmentPageRepository _apartmentPageRepository;
    private ImageService _imageService;
    private CreateApartmentPageRequestHandler _handler;
    private ILogger<ApartmentPageService> _logger; // Добавлено поле для хранения реального логгера

    public CreateApartmentPageRequestHandlerTests()
    {
        _logger = new Logger<ApartmentPageService>(new LoggerFactory()); // Создание реального логгера
        _apartmentPageRepository = new Mock<ApartmentPageRepository>(_database).Object;
        _imageService = new Mock<ImageService>().Object;
        _apartmentPageService =
            new ApartmentPageService(_apartmentPageRepository, _logger); // Используем реальный логгер
        _handler = new CreateApartmentPageRequestHandler(_imageService, _apartmentPageService,
            new CreatePageRequestToApartmentPageMapper(), new StringToGuidMapper());
    }

    [Fact]
    public async Task CreateApartmentPageRequestHandler_Success()
    {
        await _handler.Handle(new CreateApartmentPageRequest()
        {
            OwnerId = Guid.NewGuid().ToString(),
            Area = 100,
            ApartmentType = ApartmentType.Flat.ToString(),
            Rooms = 1,
            Street = "Lenina",
            HouseNumber = 5,
            Building = "A",
            City = "Minsk",
            Title = "Студия в Минске",
            DailyPrice = 500,
            IsAvailableForPurchase = true,
            Price = 100000,
            PhoneNumber = "+375445983750",
            Images = new List<IFormFile>()
        }, CancellationToken.None);
    }
    
    [Fact]
    public async Task CreateApartmentPageRequestHandler_InvalidId()
    {
        var invalidOwnerId = "12345"; // Установите некорректный OwnerId

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await _handler.Handle(new CreateApartmentPageRequest()
            {
                OwnerId = invalidOwnerId,
                Area = 100,
                ApartmentType = ApartmentType.Flat.ToString(),
                Rooms = 1,
                Street = "Lenina",
                HouseNumber = 5,
                Building = "A",
                City = "Minsk",
                Title = "Студия в Минске",
                DailyPrice = 500,
                IsAvailableForPurchase = true,
                Price = 100000,
                PhoneNumber = "+375445983750",
                Images = new List<IFormFile>()
            }, CancellationToken.None);
        });
    }
}