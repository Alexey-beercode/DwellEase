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

public class CreateApartmentPageRequestHandler:IRequestHandler<CreateApartmentPageRequest,bool>
{
    private readonly ApartmentPageService _apartmentPageService;
    private readonly IImageService _imageService;
    private readonly CreatePageRequestToApartmentPageMapper _mapper;

    public CreateApartmentPageRequestHandler(IImageService imageService, ApartmentPageService apartmentPageService, CreatePageRequestToApartmentPageMapper mapper)
    {
        _imageService = imageService;
        _apartmentPageService = apartmentPageService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CreateApartmentPageRequest request, CancellationToken cancellationToken)
    {
        var imageServiceResponse = _imageService.UploadImage(request.Images);
        if (imageServiceResponse.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(imageServiceResponse.Description);
        }

        if (!Guid.TryParse(request.OwnerId,out Guid ownerGuidId))
        {
            throw new ("OwnerId is not valid");
        }
        if (!ApartmentType.TryParse(request.ApartmentType, out ApartmentType apartmentType))
        {
            throw new Exception("Apartment type is not valid");
        }

        var apartmentPage = _mapper.MapTo(request, imageServiceResponse.Data, ownerGuidId, apartmentType);
        await _apartmentPageService.CreateAsync(apartmentPage);
        return true;
    }
}