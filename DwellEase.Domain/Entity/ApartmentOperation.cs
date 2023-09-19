using System.Net.NetworkInformation;
using DwellEase.Domain.Enum;

namespace DwellEase.Domain.Entity;

public class ApartmentOperation
{
    public Guid Id { get; set; }
    public OperationType OperationType { get; set; }
    public Guid UserId { get; set; }
    public Guid ApartmentPageId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
}