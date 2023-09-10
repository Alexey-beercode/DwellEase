namespace DwellEase.Domain.Models;

public class Address
{
    public string Street { get; set; } = null!;
    public int HouseNumber { get; set; } 
    public string City { get; set; } = null!;
    public string District { get; set; } = null!;
    public string Building { get; set; } = null!;
}
