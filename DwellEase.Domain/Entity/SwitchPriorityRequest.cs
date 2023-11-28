using DwellEase.Domain.Enum;

namespace DwellEase.Domain.Entity;

public class SwitchPriorityRequest
{
    public Guid Id { get; set; }
    public PriorityType NewType { get; set; }
    public Guid UserId { get; set; }
    public Guid ApartmentPageId { get; set; }
    public bool IsApproved { get; set; }
    
}