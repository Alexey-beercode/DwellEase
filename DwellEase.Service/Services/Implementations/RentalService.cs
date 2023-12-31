﻿using System.Net;
using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Responses;
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

    private async Task<List<ApartmentPageRentResponse>> FindExpiredApartmentPages(List<ApartmentPage> apartmentPages)
    {
        var list = new List<ApartmentPageRentResponse>();
        foreach (var apartmentPage in apartmentPages)
        {
            var operation =
                (await await _apartmentOperationRepository.GetAll())
                .Where(a => a.ApartmentPageId == apartmentPage.Id && a.OperationType == OperationType.Rent)
                .MaxBy(op => op.StartDate);
            if (operation == null)
            {
                continue;
            }
            var timeRemaining = operation.EndDate.ToLocalTime() - DateTime.Now;
            if (timeRemaining<=TimeSpan.Zero)
            {
                apartmentPage.Status = ApartmentStatus.Available;
                await _apartmentPageRepository.Update(apartmentPage);
                continue;
            }
            list.Add(new ApartmentPageRentResponse(operation.UserId,apartmentPage.Id,timeRemaining));
        }

        return list;
    }

    public async Task<BaseResponse<List<ApartmentPageRentResponse>>> CheckAndUpdateApartmentStatus()
    {
        var response = new BaseResponse<List<ApartmentPageRentResponse>>();
        var apartmentPages =await await _apartmentPageRepository.GetAll();
        if (apartmentPages.Count==0)
        {
            response.StatusCode = HttpStatusCode.NoContent;
            response.Description = "Apartment pages are not found";
            _logger.LogError("Apartment pages are not found");
            return response;
        }

        var expiredPages =await FindExpiredApartmentPages(apartmentPages);
        if (expiredPages.Count==0)
        {
            response.StatusCode = HttpStatusCode.NoContent;
            response.Description = "Apartment pages with not ended rent are not found";
            _logger.LogError("Apartment pages with not ended rent are not found");
            return response;   
        }
        response.StatusCode = HttpStatusCode.OK;
        response.Data = expiredPages;
        _logger.LogInformation("Successfully check and update apartment pages status");
        return response;
    }
}