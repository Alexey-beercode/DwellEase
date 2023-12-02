using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Service.Queries.Creator;
using DwellEase.Service.Services.Implementations;
using DwellEase.Shared.Mappers;
using MediatR;

namespace DwellEase.Service.Handlers.Creator;

public class GetOperationsByPagesOwnerQueryHandler:IRequestHandler<GetOperationsByPagesOwnerQuery, List<ApartmentOperation>>
{
    private readonly ApartmentOperationService _apartmentOperationService;
    private readonly ApartmentPageService _apartmentPageService;
    private readonly StringToGuidMapper _mapper;

    public GetOperationsByPagesOwnerQueryHandler(ApartmentOperationService apartmentOperationService, ApartmentPageService apartmentPageService, StringToGuidMapper mapper)
    {
        _apartmentOperationService = apartmentOperationService;
        _apartmentPageService = apartmentPageService;
        _mapper = mapper;
    }

    public async Task<List<ApartmentOperation>> Handle(GetOperationsByPagesOwnerQuery request, CancellationToken cancellationToken)
    {
        var response = await _apartmentOperationService.GetAllAsync();
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(response.Description);
        }
        
        var ownerPagesResponse = await _apartmentPageService.GetByOwnerAsync(_mapper.MapTo(request.Id));
        if (ownerPagesResponse.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(ownerPagesResponse.Description);
        }
        List<Guid> pageIds = ownerPagesResponse.Data.Select(p => p.Id).ToList();

        return response.Data
            .Where(op => pageIds.Contains(op.Id))
            .ToList();
    }
}