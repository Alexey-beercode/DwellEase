using System.Net;
using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Services.Implementations;

public class RentalService
{
    private readonly ApartmentOperationRepository _apartmentOperationRepository;
    private readonly ApartmentPageRepository _apartmentPageRepository;
    private readonly ILogger<RentalService> _logger;

    public RentalService(ApartmentOperationRepository apartmentOperationRepository, ApartmentPageRepository apartmentPageRepository, ILogger<RentalService> logger)
    {
        _apartmentOperationRepository = apartmentOperationRepository;
        _apartmentPageRepository = apartmentPageRepository;
        _logger = logger;
    }

    public async Task<BaseResponse<List<ApartmentPageRentInfo>>> CheckAndUpdateApartmentStatus()
    {
        var response = new BaseResponse<System.Collections.Generic.List<ApartmentPageRentInfo>>();
        var list = new List<ApartmentPageRentInfo>();
        var apartmentPages =await await _apartmentPageRepository.GetAll();
        if (apartmentPages.Count==0)
        {
            response.StatusCode = HttpStatusCode.NoContent;
            response.Description = "Apartmentpages are not found";
            _logger.LogError("Apartmentpages are not found");
            return response;
        }

        foreach (var apartmentPage in apartmentPages)
        {
            var operation =
                (await await _apartmentOperationRepository.GetAll())
                .Where(a => a.ApartmentPage == apartmentPage && a.OperationType == OperationType.Rent)
                .MaxBy(op => op.StartDate);
            if (operation == null)
            {
                continue;
            }
            var timeRemaining = operation.EndDate - DateTime.Now;
            if (timeRemaining<=TimeSpan.Zero)
            {
                apartmentPage.Status = ApartmentStatus.Available;
                await _apartmentPageRepository.Update(apartmentPage);
                continue;
            }
            list.Add(new ApartmentPageRentInfo(operation.UserId,apartmentPage.Id,timeRemaining));
        }

        if (list.Count==0)
        {
            response.StatusCode = HttpStatusCode.NoContent;
            response.Description = "Apartmentpages with not ended rent are not found";
            _logger.LogError("Apartmentpages with not ended rent are not found");
            return response;   
        }
        response.StatusCode = HttpStatusCode.OK;
        response.Data = list;
        _logger.LogInformation("Successfully check and update apartmentpages status");
        return response;
    }
}