using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Services.Implementations;
using DwellEase.Service.Services.Interfaces;
using DwellEase.Shared.Mappers;
using MediatR;

namespace DwellEase.Service.Handlers.Creator;

public class CreateApartmentPageRequestHandler : IRequestHandler<CreateApartmentPageRequest, bool>
{
    private readonly ApartmentPageService _apartmentPageService;
    private readonly IImageService _imageService;
    private readonly CreatePageRequestToApartmentPageMapper _pageMapper;
    private readonly StringToGuidMapper _guidMapper;

    public CreateApartmentPageRequestHandler(IImageService imageService, ApartmentPageService apartmentPageService,
        CreatePageRequestToApartmentPageMapper mapper, StringToGuidMapper guidMapper)
    {
        _imageService = imageService;
        _apartmentPageService = apartmentPageService;
        _pageMapper = mapper;
        _guidMapper = guidMapper;
    }

    public async Task<bool> Handle(CreateApartmentPageRequest request, CancellationToken cancellationToken)
    {
        var imageServiceResponse = _imageService.UploadImage(request.Images);
        if (imageServiceResponse.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception(imageServiceResponse.Description);
        }
        
        var ownerGuidId = _guidMapper.MapTo(request.OwnerId);

        if (!ApartmentType.TryParse(request.ApartmentType, out ApartmentType apartmentType))
        {
            throw new Exception("Apartment type is not valid");
        }
        var apartmentPage = _pageMapper.MapTo(request, imageServiceResponse.Data, ownerGuidId, apartmentType);
        await _apartmentPageService.CreateAsync(apartmentPage);
        return true;
    }
}