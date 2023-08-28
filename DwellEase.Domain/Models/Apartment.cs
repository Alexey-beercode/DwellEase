using DwellEase.Domain.Enum;

namespace DwellEase.Domain.Models;

public class Apartment
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Rooms { get; set; }
    public double Area { get; set; }
    public string Location { get; set; }
    public ApartmentType ApartmentType { get; set; }
}