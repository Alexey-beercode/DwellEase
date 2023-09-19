using System.Net;
using System.Security.Claims;
using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Services.Implementations;

public class ApartmentPageService
{
    private readonly ApartmentPageRepository _apartmentPageRepository;
    private readonly ILogger<ApartmentPageService> _logger;

    public ApartmentPageService(ApartmentPageRepository apartmentPageRepository, ILogger<ApartmentPageService> logger, UserManager<User> userManager)
    {
        _apartmentPageRepository = apartmentPageRepository;
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
    
    public async Task<BaseResponse<bool>> CreateApartmentPageAsync(ApartmentPage apartmentPageModel,Guid ownerId)
    {
        var response = new BaseResponse<bool>();
        var apartmentPage = new ApartmentPage()
        {
            Apartment = apartmentPageModel.Apartment,
            ApprovalStatus = ListingApprovalStatus.Pending,
            Date = DateOnly.FromDateTime(DateTime.Today),
            DaylyPrice = apartmentPageModel.DaylyPrice,
            Images = apartmentPageModel.Images,
            IsAvailableForPurchase = apartmentPageModel.IsAvailableForPurchase,
            OwnerId = ownerId,
            PhoneNumber = apartmentPageModel.PhoneNumber,
            Price = apartmentPageModel.Price
        };
        await _apartmentPageRepository.Create(apartmentPage);
        response.Data = true;
        response.StatusCode = HttpStatusCode.OK;
        _logger.LogInformation("Successfully create apartmentpage");
        return response;
    }

    public async Task<BaseResponse<ApartmentPage>> GetApartmentPageByIdAsync(Guid id)
    {
        var response = new BaseResponse<ApartmentPage>();
        var apartmentPage = await await _apartmentPageRepository.GetById(id);
        if (apartmentPage==null)
        {
            return HandleNotFound<ApartmentPage>($"Apartmeentpage with id: {id} not found");
        }

        response.StatusCode = HttpStatusCode.OK;
        response.Data = apartmentPage;
        _logger.LogInformation("Successfully get Apartmentpage by id");
        return response;
    }

    public async Task<BaseResponse<bool>> EditApartmentPageAsync(ApartmentPage apartmentPage)
    {
        var response = new BaseResponse<bool>();
        var newApartmentPage = await await _apartmentPageRepository.GetById(apartmentPage.Id);
        if (newApartmentPage == null)
        {
            return HandleNotFound<bool>($"Apartmeentpage with id: {apartmentPage.Id} not found");
        }
        newApartmentPage = apartmentPage;
        await _apartmentPageRepository.Update(newApartmentPage);
        response.Data = true;
        response.StatusCode = HttpStatusCode.OK;
        _logger.LogInformation($"Successfully update apartmentpage with id: {apartmentPage.Id}");
        return response;
    }

    public async Task<BaseResponse<List<ApartmentPage>>> GetApartmentPagesByOwnerAsync(Guid ownerId)
    {
        var response = new BaseResponse<List<ApartmentPage>>();
        var apartmentPages = (await await _apartmentPageRepository.GetAll()).Where(a => a.OwnerId == ownerId).ToList();
        if (apartmentPages.Count==0)
        {
            return HandleNotFound<List<ApartmentPage>>($"Apartmeentpages with owner id: {ownerId} are not found");
        }

        response.Data = apartmentPages;
        response.StatusCode = HttpStatusCode.OK;
        _logger.LogInformation($"Successfully get aprtmentpages by owner id: {ownerId}");
        return response;
    }

    public async Task<BaseResponse<List<ApartmentPage>>> GetApartmentPagesAsync()
    {
        var response = new BaseResponse<List<ApartmentPage>>();
        var apartmentPages = await await _apartmentPageRepository.GetAll();
        if (apartmentPages.Count==0)
        {
            return HandleNotFound<List<ApartmentPage>>("Apartmeentpages are not found");
        }

        response.StatusCode = HttpStatusCode.OK;
        response.Data = apartmentPages;
        _logger.LogInformation("Successfully get all apartmentpages");
        return response;
    }

    public async Task<BaseResponse<bool>> DeleteApartmentPageAsync(Guid id)
    {
        var response = new BaseResponse<bool>();
        var apartmentPage = await await _apartmentPageRepository.GetById(id);
        if (apartmentPage==null)
        {
            return HandleNotFound<bool>($"Apartmeentpage with id: {id} not found");
        }

        await _apartmentPageRepository.Delete(id);
        response.StatusCode = HttpStatusCode.OK;
        response.Data = true;
        return response;
    }

    public async Task<BaseResponse<List<ApartmentPage>>> GetApartmentPagesByStatusAsync(ApartmentStatus status)
    {
        var response = new BaseResponse<List<ApartmentPage>>();
        var apartmentPages = (await await _apartmentPageRepository.GetAll()).Where(a => a.Status == status).ToList();
        if (apartmentPages.Count==0)
        {
            return HandleNotFound<List<ApartmentPage>>($"Apartmentpages with status: {status} are not found");
        }

        response.StatusCode = HttpStatusCode.OK;
        response.Data = apartmentPages;
        return response;
    }

    public async Task<BaseResponse<List<ApartmentPage>>> GetApartmentPagesByApprovalStatusAsync(ListingApprovalStatus approvalStatus)
    {
        var response = new BaseResponse<List<ApartmentPage>>();
        var apartmentPages = (await await _apartmentPageRepository.GetAll()).Where(a => a.ApprovalStatus==approvalStatus).ToList();
        if (apartmentPages.Count==0)
        {
            return HandleNotFound<List<ApartmentPage>>($"Apartmentpages with approvalstatus: {approvalStatus} are not found");
        }

        response.StatusCode = HttpStatusCode.OK;
        response.Data = apartmentPages;
        return response;
    }

    public async Task<BaseResponse<List<ApartmentPage>>> GetApartmentPageForPurchaseAsync()
    {
        var response = new BaseResponse<List<ApartmentPage>>();
        var apartmentPages = (await await _apartmentPageRepository.GetAll()).Where(a => a.IsAvailableForPurchase).ToList();
        if (apartmentPages.Count==0)
        {
            return HandleNotFound<List<ApartmentPage>>($"Apartmentpages available for purchase are not found");
        }

        response.StatusCode = HttpStatusCode.OK;
        response.Data = apartmentPages;
        return response;
    }

    public async Task<BaseResponse<List<ApartmentPage>>> GetApartmentPageOnlyForRentAsync()
    {
        var response = new BaseResponse<List<ApartmentPage>>();
        var apartmentPages = (await await _apartmentPageRepository.GetAll()).Where(a => a.IsAvailableForPurchase==false).ToList();
        if (apartmentPages.Count==0)
        {
            return HandleNotFound<List<ApartmentPage>>($"Apartmentpages available only for rent are not found");
        }

        response.StatusCode = HttpStatusCode.OK;
        response.Data = apartmentPages;
        return response;
    }

    public async Task<BaseResponse<bool>> EditApartmentPageApprovalStatusAsync(Guid id, ListingApprovalStatus newStatus)
    {
        var response = new BaseResponse<bool>();
        var apartmentPage = await await _apartmentPageRepository.GetById(id);
        if (apartmentPage==null)
        {
            return HandleNotFound<bool>($"Apartmeentpage with id: {id} not found");
        }

        apartmentPage.ApprovalStatus = newStatus;
        response.Data = true;
        response.StatusCode = HttpStatusCode.OK;
        return response;
    }

    public async Task<BaseResponse<bool>> EditApartmentPagePriorityTypeAsync(Guid id, PriorityType newPriorityType)
    {
        var response = new BaseResponse<bool>();
        var apartmentPage = await await _apartmentPageRepository.GetById(id);
        if (apartmentPage==null)
        {
            return HandleNotFound<bool>($"Apartmeentpage with id: {id} not found");
        }

        apartmentPage.PriorityType=newPriorityType;
        response.Data = true;
        response.StatusCode = HttpStatusCode.OK;
        return response;
    }

}