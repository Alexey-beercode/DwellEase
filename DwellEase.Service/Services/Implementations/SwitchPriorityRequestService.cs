using System.Net;
using DwellEase.DataManagement.Repositories.Interfaces;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Services.Implementations;

public class SwitchPriorityRequestService
{
    private readonly ILogger<SwitchPriorityRequestService> _logger;
    private readonly IBaseRepository<SwitchPriorityRequest> _repository;
    private readonly ApartmentPageService _apartmentPageService;

    public SwitchPriorityRequestService(ILogger<SwitchPriorityRequestService> logger, IBaseRepository<SwitchPriorityRequest> repository, ApartmentPageService apartmentPageService)
    {
        _logger = logger;
        _repository = repository;
        _apartmentPageService = apartmentPageService;
    }

    private BaseResponse<T> HandleError<T>(string description,HttpStatusCode error)
    {
        var response = new BaseResponse<T>
        {
            StatusCode = error,
            Description = description
        };
        _logger.LogError(description);
        return response;
    }
    public async Task<BaseResponse<List<SwitchPriorityRequest>>> GetAllAsync()
    {
        var switchPriorityRequests = await await _repository.GetAll();
        if (switchPriorityRequests.Count==0)
        {
            return HandleError<List<SwitchPriorityRequest>>("Requests are not found", HttpStatusCode.NoContent);
        }
        return new BaseResponse<List<SwitchPriorityRequest>> { Data = switchPriorityRequests, StatusCode = HttpStatusCode.OK };
    }

    public async Task<BaseResponse<bool>> Approve(Guid id)
    {
        var request = await await _repository.GetById(id);
        if (request is null)
        {
            return HandleError<bool>("Request is not found", HttpStatusCode.NoContent);
        }
        request.IsApproved = true;
        await _repository.Update(request);
        var apartmentPageResponse = await _apartmentPageService.GetByIdAsync(request.ApartmentPageId);
        if (apartmentPageResponse.StatusCode!=HttpStatusCode.OK)
        {
            return HandleError<bool>("Apartment page is not found", HttpStatusCode.NoContent);
        }

        var apartmentPage = apartmentPageResponse.Data;
        apartmentPage.PriorityType = request.NewType;
        await _apartmentPageService.EditAsync(apartmentPage);
        return new BaseResponse<bool>() { StatusCode = HttpStatusCode.OK };
    }

    public async Task Create(SwitchPriorityRequest request)=> await _repository.Create(request);

}