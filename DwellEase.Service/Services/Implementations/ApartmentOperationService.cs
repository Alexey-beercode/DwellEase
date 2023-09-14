using System.Net;
using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Services.Implementations;

public class ApartmentOperationService
{
    private readonly ApartmentOperationRepository _apartmentOperationRepository;
    private readonly ILogger<ApartmentOperationRepository> _logger;

    public ApartmentOperationService(ApartmentOperationRepository apartmentOperationRepository, ILogger<ApartmentOperationRepository> logger)
    {
        _apartmentOperationRepository = apartmentOperationRepository;
        _logger = logger;
    }
    
    private BaseResponse<T> HandleNotFound<T>(string description)
    {
        var response = new BaseResponse<T>
        {
            StatusCode = HttpStatusCode.NoContent,
            Description = description
        };
        _logger.LogError(description);
        return response;
    }

    public async Task<BaseResponse<bool>> CreatePurchaseOperationAsync(ApartmentOperation apartmentOperation ,Guid userId)
    {
        var response = new BaseResponse<bool>();
        if (apartmentOperation.OperationType!=OperationType.Purchase)
        {
            return HandleNotFound<bool>("Operationtype of model is not purchase");
        }

        var newApartmentOperation = new ApartmentOperation()
        {
            ApartmentPage = apartmentOperation.ApartmentPage,
            OperationType = OperationType.Purchase,
            UserId = userId,
            Price = apartmentOperation.Price
        };
        await _apartmentOperationRepository.Create(newApartmentOperation);
        response.Data = true;
        response.StatusCode = HttpStatusCode.OK;
        _logger.LogInformation("Successfully create aprtmentoperation");
        return response;
    }
    
    public async Task<BaseResponse<bool>> CreateRentOperationAsync(ApartmentOperation apartmentOperation ,Guid userId)
    {
        var response = new BaseResponse<bool>();
        if (apartmentOperation.OperationType!=OperationType.Rent)
        {
            return HandleNotFound<bool>("Operationtype of model is not rent");
        }

        var newApartmentOperation = new ApartmentOperation()
        {
            ApartmentPage = apartmentOperation.ApartmentPage,
            OperationType = OperationType.Rent,
            StartDate = DateTime.Now,
            EndDate = apartmentOperation.EndDate,
            UserId = userId,
            Price = apartmentOperation.Price
        };
        await _apartmentOperationRepository.Create(newApartmentOperation);
        response.Data = true;
        response.StatusCode = HttpStatusCode.OK;
        return response;
    }

    public async Task<BaseResponse<bool>> EditApartmentOperationEndDateAsync(Guid id, DateTime newEndDate)
    {
        var response = new BaseResponse<bool>();
        var apartmentOperation = await await _apartmentOperationRepository.GetById(id);
        if (apartmentOperation==null)
        {
            return HandleNotFound<bool>($"Apartmentoperation with id: {id} not found");
        }
        apartmentOperation.EndDate = newEndDate;
        await _apartmentOperationRepository.Update(apartmentOperation);
        _logger.LogInformation($"Successfully edit aprtmentoperation enddate with {id} to {newEndDate} ");
        response.Data = true;
        response.StatusCode = HttpStatusCode.OK;
        return response;
    }

    public async Task<BaseResponse<List<ApartmentOperation>>> GetApartmenOperationsAsync()
    {
        var response = new BaseResponse<List<ApartmentOperation>>();
        var apartmentOperations = await await _apartmentOperationRepository.GetAll();
        if (apartmentOperations.Count==0)
        {
            return HandleNotFound<List<ApartmentOperation>>("Apartmentoperations are not found");
        }
        response.Data = apartmentOperations;
        response.StatusCode = HttpStatusCode.OK;
        _logger.LogInformation("Successfully get aprtmentoperations ");
        return response;
    }

    public async Task<BaseResponse<bool>> EditApartmentOperationAsync(ApartmentOperation apartmentOperation)
    {
        var response = new BaseResponse<bool>();
        var operation = await await _apartmentOperationRepository.GetById(apartmentOperation.Id);
        if (operation==null)
        {
            return HandleNotFound<bool>($"Apartmentoperation with id: {apartmentOperation.Id} not found");
        }

        operation.OperationType = apartmentOperation.OperationType;
        operation.ApartmentPage = apartmentOperation.ApartmentPage;
        operation.StartDate = apartmentOperation.StartDate;
        operation.EndDate = apartmentOperation.EndDate;
        operation.Price = apartmentOperation.Price;
        operation.UserId = apartmentOperation.UserId;
        response.Data = true;
        response.StatusCode = HttpStatusCode.OK;
        return response;
    }

    public async Task<BaseResponse<bool>> DeleteApartmenOperationAsync(Guid id)
    {
        var response = new BaseResponse<bool>();
        var apartmentOperation = await await _apartmentOperationRepository.GetById(id);
        if (apartmentOperation==null)
        {
            return HandleNotFound<bool>($"Apartmentoperation with id: {apartmentOperation.Id} not found");
        }

        await _apartmentOperationRepository.Delete(id);
        response.Data = true;
        response.StatusCode = HttpStatusCode.OK;
        return response;
    }
    
}