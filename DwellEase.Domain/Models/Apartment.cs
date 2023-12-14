using DwellEase.Domain.Enum;

namespace DwellEase.Domain.Models;

public class Apartment
{
    public int Rooms { get; set; }
    public double Area { get; set; }
    public Address Address { get; set; } = null!;
    public ApartmentType ApartmentType { get; set; }
}