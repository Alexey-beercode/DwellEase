using System.Net;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using MediatR;

namespace DwellEase.Service.Handlers.Admin;

public class UpdateApprovalStatusRequestHandler:IRequestHandler<UpdateApprovalStatusRequest,bool>
{
    private readonly ApartmentPageService _apartmentPageService;
    private readonly StringToGuidMapper _guidMapper;

    public UpdateApprovalStatusRequestHandler(ApartmentPageService apartmentPageService, StringToGuidMapper guidMapper)
    {
        _apartmentPageService = apartmentPageService;
        _guidMapper = guidMapper;
    }

    public async Task<bool> Handle(UpdateApprovalStatusRequest request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(request.NewStatus,out ListingApprovalStatus newStatus))
        {
            throw new Exception("Ivalid status");
        }

        var guidId = _guidMapper.MapTo(request.ApartmentPageId);
        var response = await _apartmentPageService.GetByIdAsync(guidId);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new(response.Description);
        }

        var apartmentPage = response.Data;
        apartmentPage.ApprovalStatus = newStatus;
        await _apartmentPageService.EditApprovalStatusAsync(guidId, newStatus);
        return true;
    }
}